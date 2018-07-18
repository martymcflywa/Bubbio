using System;

namespace Bubbio.Core.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Specify collection name with this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionName : Attribute
    {
        public string Name { get; set; } = "Bubbio";

        public CollectionName()
        {
        }

        public CollectionName(string name)
        {
            Name = name;
        }
    }
}