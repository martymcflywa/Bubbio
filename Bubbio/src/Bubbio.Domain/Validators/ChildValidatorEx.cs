using Bubbio.Core.Contracts;
using Bubbio.Core.Exceptions;
using Bubbio.Core.Helpers;

namespace Bubbio.Domain.Validators
{
    public static class ChildValidatorEx
    {
        public static IChild Validate(this IChild child)
        {
            return child
                .ValidateId()
                .ValidateParentId()
                .ValidateName();
        }

        private static IChild ValidateId(this IChild child)
        {
            if (child.Id.IsEmpty())
                throw new InvalidIdException(child.Name);

            return child;
        }

        private static IChild ValidateParentId(this IChild child)
        {
            if (child.ParentId.IsEmpty())
                throw new OrphanedChildException(child);

            return child;
        }

        private static IChild ValidateName(this IChild child)
        {
            child.Name = child.Name.ValidateName();
            return child;
        }
    }
}