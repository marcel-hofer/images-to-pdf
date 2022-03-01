#!/bin/sh
set -eu

cd "$(dirname "$0")/.."
. ./tools/_config.sh

echo "RESTORE"
dotnet restore ./src/ImagesToPdf.sln

echo "PUBLISH"
echo "  used version:" "$exact_version"
rm -rf ./build/*
dotnet publish ./src/ImagesToPdf.sln  -o ./build -p:version="$exact_version" --configuration Release
echo
