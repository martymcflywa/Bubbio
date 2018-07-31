using Bubbio.Core.Contracts.Events;
using Bubbio.Domain.Validators;
using FluentAssertions;

namespace Bubbio.Domain.Tests.Scenarios
{
    public abstract class TransitionValidatorTestsBase
    {
        private readonly TransitionValidator _validator;
        private ITransitionEvent _previous;
        private ITransitionEvent _current;
        private bool _isValid;

        protected TransitionValidatorTestsBase()
        {
            _validator = new TransitionValidator();
        }

        protected void PreviousEventIs(ITransitionEvent previous) =>
            _previous = previous;

        protected void CurrentEventIs(ITransitionEvent current) =>
            _current = current;

        protected void TransitionValidated() =>
            _isValid = _validator.IsValid(_previous, _current);

        protected void IsValid(bool expected) =>
            _isValid.Should().Be(expected);
    }
}