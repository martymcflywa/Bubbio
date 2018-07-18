#!/bin/bash
set -e
source ./scripts/env.sh

TESTS=$PROJECT/test/**
TEST=*.Tests

for t in $TESTS; do
    if [[ $t == $TEST ]]; then
        echo "Executing test run for $t";
        dotnet test $t;
    fi
done