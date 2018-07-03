using System;
using Bubbio.Core;
using Bubbio.Core.Exceptions;
using Bubbio.Domain.Validators;
using Bubbio.Tests.Core.Builders;
using Xunit;

namespace Bubbio.Domain.Tests.Scenarios
{
    public class ParentExTestsBase
    {
        private Parent _parentPreValidation;
        private Parent _parentPostValidation;

        protected string NamePreValidation;
        protected string NamePostValidation;

        protected void ParentIsValidated()
        {
            try
            {
                _parentPostValidation = _parentPreValidation.Validate();
            }
            catch (InvalidNameException) {}
            catch (InvalidIdException) {}
        }

        protected void ParentWithFirstName(string name) =>
            _parentPreValidation = new ParentBuilder()
                .WithFirstName(name)
                .Build();

        protected void FirstNameIsFormatted(string expected) =>
            Assert.Equal(expected, _parentPostValidation.Name.First);

        protected void ParentWithMiddleName(string name) =>
            _parentPreValidation = new ParentBuilder()
                .WithMiddleName(name)
                .Build();

        protected void MiddleNameIsFormatted(string expected) =>
            Assert.Equal(expected, _parentPostValidation.Name.Middle);

        protected void ParentWithLastName(string name) =>
            _parentPreValidation = new ParentBuilder()
                .WithLastName(name)
                .Build();

        protected void LastNameIsFormatted(string expected) =>
            Assert.Equal(expected, _parentPostValidation.Name.Last);

        protected void ParentWithoutId() =>
            _parentPreValidation = new ParentBuilder()
                .WithId(new Guid())
                .Build();

        protected void ParentIsInvalid() =>
            Assert.Null(_parentPostValidation);
    }
}