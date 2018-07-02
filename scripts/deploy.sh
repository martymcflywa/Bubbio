#!/bin/bash
set -e
source ./scripts/env.sh

dotnet build --no-incremental --configuration Release $PROJECT