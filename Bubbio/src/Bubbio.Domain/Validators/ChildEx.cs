using Bubbio.Core;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Core.Exceptions;
using Bubbio.Core.Helpers;

namespace Bubbio.Domain.Validators
{
    public static class ChildEx
    {
        public static Child Validate(this Child child)
        {
            return child
                .ValidateName();
        }

        private static Child ValidateName(this Child child)
        {
            var first = child.Name.First.Validate();
            var middle = child.Name.Middle.Validate();
            var last = child.Name.Last.Validate();

            if (first.IsEmpty())
                throw new InvalidNameException(NamePosition.First, child.Name.First);
            if (middle.IsEmpty() && !child.Name.Middle.IsEmpty())
                throw new InvalidNameException(NamePosition.Middle, child.Name.Middle);
            if (last.IsEmpty())
                throw new InvalidNameException(NamePosition.Last, child.Name.Last);

            child.Name = new Name {First = first, Middle = middle, Last = last};
            return child;
        }
    }
}