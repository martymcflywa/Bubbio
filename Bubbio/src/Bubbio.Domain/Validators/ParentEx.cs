using Bubbio.Core;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Core.Exceptions;
using Bubbio.Core.Helpers;

namespace Bubbio.Domain.Validators
{
    public static class ParentEx
    {
        public static Parent Validate(this Parent parent)
        {
            return parent
                .ValidateId()
                .ValidateName();
        }

        private static Parent ValidateId(this Parent parent)
        {
            if (parent.Id.IsEmpty())
                throw new InvalidIdException(parent.Name);

            return parent;
        }

        private static Parent ValidateName(this Parent parent)
        {
            var first = parent.Name.First.Validate();
            var middle = parent.Name.Middle.Validate();
            var last = parent.Name.Last.Validate();

            if (first.IsEmpty())
                throw new InvalidNameException(NamePosition.First, parent.Name.First);
            if (middle.IsEmpty() && !parent.Name.Middle.IsEmpty())
                throw new InvalidNameException(NamePosition.Middle, parent.Name.Middle);
            if (last.IsEmpty())
                throw new InvalidNameException(NamePosition.Last, parent.Name.Last);

            parent.Name = new Name {First = first, Middle = middle, Last = last};
            return parent;
        }
    }
}