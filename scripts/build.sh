#!/bin/bash
set -e
source ./scripts/env.sh

dotnet build --no-incremental $PROJECT