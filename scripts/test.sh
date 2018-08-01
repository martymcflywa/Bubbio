#!/bin/bash

source ./scripts/env.sh

tests=$project/test/**
test=*.Tests

# setup mongodb for local
if [ $isLocal == true ]; then
    echo "Local test run"
    echo "Setting up mongodb"
    mongod --fork --dbpath ~/Documents/dev/database/bubbio/mongo --logpath /dev/null

    echo "Waiting for mongodb to start"
    until nc -z localhost 27017; do
        sleep 1
    done
    echo "Mongodb started"
fi

# run tests
for t in $tests; do
    if [[ $t == $test ]]; then
        echo "Executing test run for $t";
        dotnet test $t;
    fi
done

# clean mongodb for local
if [ $isLocal == true ]; then
    echo "Cleaning up test databases"
    mongo --eval \
    "db.adminCommand({ \
        listDatabases: 1, \
        nameOnly: true, \
        filter: { \
            'name': /^test-.*/ \
        } \
    }).databases \
    .forEach(function(d) { \
        db.getSiblingDB(d.name).dropDatabase() \
    })"
    echo "Shutting down mongodb"
    mongo admin --eval "db.shutdownServer()"
fi