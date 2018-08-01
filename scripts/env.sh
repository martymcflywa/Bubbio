#!/bin/bash
set -e

if [ -z "$CI_PROJECT_NAME" ]; then
    project=Bubbio
    isLocal=true
else
    project="$CI_PROJECT_NAME"
    isLocal=false
fi