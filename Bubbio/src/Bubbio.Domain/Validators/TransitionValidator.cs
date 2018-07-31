using Bubbio.Core.Abstraction;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Core.Contracts.Events;

namespace Bubbio.Domain.Validators
{
    public class TransitionValidator : ITransitionValidator
    {
        public bool IsValid(ITransitionEvent previous, ITransitionEvent current)
        {
            if (previous == null && current == null)
                return false;

            switch (current.Transition)
            {
                case Transition.Start when previous == null || previous.Transition.Equals(Transition.End):
                case Transition.End when previous != null && previous.Transition.Equals(Transition.Start):
                    return true;
                default:
                    return false;
            }
        }
    }
}