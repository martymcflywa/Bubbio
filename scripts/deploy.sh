#!/bin/bash
set -e

dotnet build --no-incremental --configuration Release $CI_PROJECT_NAME