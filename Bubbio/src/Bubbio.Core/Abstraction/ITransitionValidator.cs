using Bubbio.Core.Contracts.Events;

namespace Bubbio.Core.Abstraction
{
    public interface ITransitionValidator
    {
        bool IsValid(ITransitionEvent previous, ITransitionEvent current);
    }
}