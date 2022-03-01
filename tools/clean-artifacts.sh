#!/bin/sh
ARTIFACTS=$(curl -X GET -H "Authorization: $auth" https://api.github.com/repos/$repository/actions/artifacts)

URLS=$(node -pe '

    const minArtifactsToKeep = process.argv[2];
    const minArtifactsDaysToKeep = process.argv[3];
    const minArtifactsLifetime = new Date(new Date().setDate(new Date().getDate()-minArtifactsDaysToKeep))

    const artifacts = JSON.parse(process.argv[1]).artifacts;
    artifacts.sort((a, b) => (a.created_at > b.created_at) ? 1 : -1);
    artifacts.reverse();

    for (let i = 0; i < minArtifactsToKeep && artifacts.length > 0; i++) {
        artifacts.splice(0, 1);
    }

    while (artifacts.length > 0) {
        const artifact = artifacts[0];
        if (new Date(artifact.created_at) > minArtifactsLifetime) {
            artifacts.splice(0, 1);
            continue;
        }

        break;
    }

    const urls = artifacts.map(e => e.url);
    urls.join("\n");

' "$ARTIFACTS" "$minArtifactsToKeep" "$minArtifactsDaysToKeep")

echo "Deleting the following artifacts:"
echo "$URLS"

echo "Begin"
echo "$URLS" | xargs echo -n | xargs --max-lines=1 --null -d " " curl -X DELETE -H "Authorization: $auth"

echo "Done."