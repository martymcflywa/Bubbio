using System;
using Bubbio.Core.Contracts;
using Bubbio.Core.Exceptions;
using Bubbio.Domain.Validators;
using Bubbio.Tests.Core.Builders;
using Xunit;

namespace Bubbio.Domain.Tests.Scenarios
{
    public class ChildExTestsBase
    {
        private IChild _childPreValidation;
        private IChild _childPostValidation;

        protected string NamePreValidation;
        protected string NamePostValidation;

        protected void ChildIsValidated()
        {
            try
            {
                _childPostValidation = _childPreValidation.Validate();
            }
            catch (InvalidNameException) {}
            catch (OrphanedChildException) {}
        }

        protected void ChildWithFirstName(string name) =>
            _childPreValidation = new ChildBuilder()
                .WithFirstName(name)
                .Build();

        protected void FirstNameIsFormatted(string expected) =>
            Assert.Equal(expected, _childPostValidation.Name.First);

        protected void ChildWithMiddleName(string name) =>
            _childPreValidation = new ChildBuilder()
                .WithMiddleName(name)
                .Build();

        protected void MiddleNameIsFormatted(string expected) =>
            Assert.Equal(expected, _childPostValidation.Name.Middle);

        protected void ChildWithLastName(string name) =>
            _childPreValidation = new ChildBuilder()
                .WithLastName(name)
                .Build();

        protected void LastNameIsFormatted(string expected) =>
            Assert.Equal(expected, _childPostValidation.Name.Last);

        protected void ChildWithoutParent() =>
            _childPreValidation = new ChildBuilder()
                .WithParentId(new Guid())
                .Build();

        protected void ChildIsInvalid() =>
            Assert.Null(_childPostValidation);
    }
}