using Bubbio.Core.Contracts;
using Bubbio.Core.Exceptions;
using Bubbio.Core.Helpers;

namespace Bubbio.Domain.Validators
{
    public static class ParentValidatorEx
    {
        public static IParent Validate(this IParent parent)
        {
            return parent
                .ValidateId()
                .ValidateName();
        }

        private static IParent ValidateId(this IParent parent)
        {
            if (parent.Id.IsEmpty())
                throw new InvalidIdException(parent.Name);

            return parent;
        }

        private static IParent ValidateName(this IParent parent)
        {
            parent.Name = parent.Name.ValidateName();
            return parent;
        }
    }
}