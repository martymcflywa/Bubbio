﻿using System;
using Bubbio.Core.Attributes;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Core.Contracts.Events;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.MongoDb.Documents.Events
{
    [Serializable]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(BreastFeed))]
    [CollectionName]
    public class BreastFeed : TransitionEvent, IBreastFeed
    {
        [BsonRepresentation(BsonType.String)]
        public Side Side { get; set; }
    }
}