#!/bin/bash
set -e

dotnet build --no-incremental $CI_PROJECT_NAME