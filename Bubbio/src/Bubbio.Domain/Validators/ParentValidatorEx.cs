using Bubbio.Core.Contracts;

namespace Bubbio.Domain.Validators
{
    public static class ParentValidatorEx
    {
        public static IParent Validate(this IParent parent)
        {
            return parent
                .ValidateName();
        }

        private static IParent ValidateName(this IParent parent)
        {
            parent.Name = parent.Name.ValidateName();
            return parent;
        }
    }
}