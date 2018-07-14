using System;

namespace Bubbio.Repository.MongoDb.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Specify collection name with this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionName : Attribute
    {
        public string Name { get; set; }

        public CollectionName(string name)
        {
            Name = name;
        }
    }
}