#!/bin/bash

source ./scripts/env.sh

tests=$project/test/**
test=*.Tests

function mongoDbUp()
{
    nc -w1 localhost 27017
    if [ "$?" == 0 ]; then
        return 1
    fi
    return 0;
}

# setup mongodb for local
if [ $isLocal == true ]; then
    echo "Local test run"
    if mongoDbUp == 0; then
        echo "Setting up mongodb"
        mongod --fork --dbpath ~/Documents/dev/database/bubbio/mongo --logpath /dev/null
        echo "Waiting for mongodb to start"
        while mongoDbUp == 0; do
            sleep 1
        done
        echo "Mongodb started"
    fi
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