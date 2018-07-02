#!/bin/bash
set -e
source ./scripts/env.sh

TESTS_CORE=*Bubbio.Tests.Core

for p in $PROJECT/test/**; do
    if [[ "$p" != $TESTS_CORE ]]; then
        echo "Executing test run for $p";
        dotnet test $p;
    fi
done