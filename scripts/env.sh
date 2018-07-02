#!/bin/bash
if [[ -z "$CI_PROJECT_NAME" ]]; then
    PROJECT=Bubbio
else
    PROJECT="$CI_PROJECT_NAME"
fi