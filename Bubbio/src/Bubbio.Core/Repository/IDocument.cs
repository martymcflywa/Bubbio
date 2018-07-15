using System;

namespace Bubbio.Core.Repository
{
    /// <summary>
    /// A generic document to be stored in a database.
    /// </summary>
    /// <typeparam name="TKey">The primary key type.</typeparam>
    public interface IDocument<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// The primary key.
        /// </summary>
        TKey Id { get; set; }

        /// <summary>
        /// The schema version.
        /// </summary>
        int Version { get; set; }
    }
}