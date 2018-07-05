using Bubbio.Core.Contracts;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Core.Exceptions;
using Bubbio.Core.Helpers;

namespace Bubbio.Domain.Validators
{
    public static class NameValidatorEx
    {
        public static IName ValidateName(this IName name)
        {
            var first = name.First.Validate();
            var middle = name.Middle.Validate();
            var last = name.Last.Validate();

            if (first.IsEmpty())
                throw new InvalidNameException(NamePosition.First, name.First);
            if (middle.IsEmpty() && !name.Middle.IsEmpty())
                throw new InvalidNameException(NamePosition.Middle, name.Middle);
            if (last.IsEmpty())
                throw new InvalidNameException(NamePosition.Last, name.Last);

            name.First = first;
            name.Middle = middle;
            name.Last = last;
            return name;
        }
    }
}