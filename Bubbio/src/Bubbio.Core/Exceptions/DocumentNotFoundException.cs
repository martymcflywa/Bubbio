using System;
using Bubbio.Core.Repository;

namespace Bubbio.Core.Exceptions
{
    public class DocumentNotFoundException<TKey> : Exception
        where TKey : IEquatable<TKey>
    {
        private const string ErrorMessage = "not found in collection";

        public DocumentNotFoundException(IDocument<TKey> document)
            : base($"{document.GetType()} {ErrorMessage}")
        {
        }

        public DocumentNotFoundException(Type documentType)
            : base($"{documentType} {ErrorMessage}")
        {
        }

        public DocumentNotFoundException(Type documentType, TKey id)
            : base($"{documentType} with {id} {ErrorMessage}")
        {
        }
    }
}