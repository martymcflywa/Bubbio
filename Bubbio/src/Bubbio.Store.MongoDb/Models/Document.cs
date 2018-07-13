using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.Store.MongoDb.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Default basic document implementation, uses Guid as Id type.
    /// </summary>
    public class Document : IDocument
    {
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        [BsonId]
        public Guid Id { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// When this document was created, in UTC
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// When this document was modified, in UTC.
        /// Same as Created on construction.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Modified { get; set; }

        public Document()
        {
            Id = Guid.NewGuid();
            Created = DateTimeOffset.UtcNow;
            Modified = DateTimeOffset.UtcNow;
        }
    }
}