﻿using System;
using System.Reflection;
using Bubbio.Core.Repository;

namespace Bubbio.Core.Exceptions
{
    public class DocumentNotFoundException<TKey> : Exception
        where TKey : IEquatable<TKey>
    {
        private const string ErrorMessage = "not found in collection";

        public DocumentNotFoundException(IDocument<TKey> document)
            : base($"{document.GetType().Name} {ErrorMessage}")
        {
        }

        public DocumentNotFoundException(MemberInfo documentType)
            : base($"{documentType.Name} {ErrorMessage}")
        {
        }

        public DocumentNotFoundException(MemberInfo documentType, TKey id)
            : base($"{documentType.Name} with {id} {ErrorMessage}")
        {
        }
    }

    public class DocumentNotFoundException : DocumentNotFoundException<Guid>
    {
        public DocumentNotFoundException(IDocument<Guid> document) : base(document)
        {
        }

        public DocumentNotFoundException(MemberInfo documentType) : base(documentType)
        {
        }

        public DocumentNotFoundException(MemberInfo documentType, Guid id) : base(documentType, id)
        {
        }
    }
}