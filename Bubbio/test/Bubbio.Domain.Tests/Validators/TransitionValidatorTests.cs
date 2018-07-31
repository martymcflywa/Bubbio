using Bubbio.Core.Contracts.Events;
using Bubbio.Domain.Tests.Scenarios;
using Bubbio.Domain.Tests.Theories;
using TestStack.BDDfy;
using Xunit;

namespace Bubbio.Domain.Tests.Validators
{
    public class TransitionValidatorTests : TransitionValidatorTestsBase
    {
        [Theory]
        [ClassData(typeof(TransitionPreviousCurrent))]
        public void ValidateTransition(ITransitionEvent previous, ITransitionEvent current, bool expected)
        {
            this.Given(_ => PreviousEventIs(previous))
                .And(_ => CurrentEventIs(current))
                .When(_ => TransitionValidated())
                .Then(_ => IsValid(expected))
                .BDDfy();
        }
    }
}