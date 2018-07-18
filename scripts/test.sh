#!/bin/bash
set -e
source ./scripts/env.sh

TEST=*.Tests

for p in $PROJECT/test/**; do
    if [[ "$p" == $TEST ]]; then
        echo "Executing test run for $p";
        dotnet test $p;
    fi
done