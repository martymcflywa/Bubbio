#!/bin/bash
set -e

for project in $CI_PROJECT_NAME/test/**; do
    dotnet test $project;
done