#!/bin/sh
set -eu

: "${configloaded:=false}"
if [ "$configloaded" = true ]
then
  return
fi
configloaded=true

echo "SCRIPT CONFIG"
echo "-------------"
echo "load ./tools/_config.sh"

echo "detect version"
# get git version ('1.2.3-0-g4201337' or 'v1.2.3-0-g4201337'))
v=$(git describe --tags --long --always)
pattern="^v?[0-9]+(\.[0-9]+){0,3}-[0-9]+-.+$"
if echo "$v" | grep -Ev "$pattern"; then
  vv=$(git rev-list HEAD --count)
  v=v0.0.0.0-$vv-$v
fi

# Remove preceeding 'v' because this is not a valid version number (dotnet)
v2=$(echo "$v" | sed -E 's/v?(.*)/\1/')
export exact_version=$v2
echo "  exact_version: $exact_version"

v2=$(echo "$v" | sed -E 's/([0-9]+(\.[0-9]+){0,3})-.*/\1/')
export release_name=$v2
echo "  release_name: $release_name"

v2=$(echo "$v" | sed -E 's/v?([0-9]+(\.[0-9]+){0,3})-.*/\1/')
export full_version=$v2
echo "  full_version: $full_version"

v2=$(echo "$v" | sed  -E 's/.*-([0-9]+)-.*/\1/')
export commits_since_release=$v2
v2=$(printf "%04d" "$commits_since_release")
export commits_since_release_padded=$v2
echo "  commits_since_release: $commits_since_release (padded: $commits_since_release_padded)"

v2=$(echo "$v" | sed  -E 's/.*-(.*)/\1/')
export git_hash=$v2
echo "  git_hash: $git_hash"
