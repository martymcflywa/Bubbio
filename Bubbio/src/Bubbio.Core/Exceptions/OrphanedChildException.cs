﻿using System;
using Bubbio.Core.Contracts;

namespace Bubbio.Core.Exceptions
{
    public class OrphanedChildException : Exception
    {
        public OrphanedChildException(IChild child)
            : base($"Child {child.Name.ToString()} has no parent")
        {
        }
    }
}