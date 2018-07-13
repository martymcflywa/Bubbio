using System;

namespace Bubbio.Store.MongoDb.Models
{
    /// <summary>
    /// A basic document that is stored in mongodb.
    /// Documents must implement this class in order to be handled by repository.
    /// </summary>
    /// <typeparam name="TKey">The type of key, must be of IEquitable</typeparam>
    public interface IDocument<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// The primary key, must be decorated with [BsonId] attribute.
        /// </summary>
        TKey Id { get; set; }

        /// <summary>
        /// The version of the schema.
        /// </summary>
        int Version { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// The default basic document, with Guid as Id type.
    /// </summary>
    public interface IDocument : IDocument<Guid>
    {
    }
}