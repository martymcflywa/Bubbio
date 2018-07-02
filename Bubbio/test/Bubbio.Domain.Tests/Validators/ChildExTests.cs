using Bubbio.Domain.Tests.Examples;
using Bubbio.Domain.Tests.Scenarios;
using TestStack.BDDfy;
using Xunit;

namespace Bubbio.Domain.Tests.Validators
{
    public class ChildExTests : ChildExTestsBase
    {
        [Fact]
        public void ValidateFirstName()
        {
            this.Given(_ => ChildWithFirstName(NamePreValidation))
                .When(_ => ChildIsValidated())
                .Then(_ => FirstNameIsFormatted(NamePostValidation))
                .WithExamples(Names.Valid)
                .BDDfy();
        }

        [Fact]
        public void ValidateMiddleName()
        {
            this.Given(_ => ChildWithMiddleName(NamePreValidation))
                .When(_ => ChildIsValidated())
                .Then(_ => MiddleNameIsFormatted(NamePostValidation))
                .WithExamples(Names.Valid)
                .BDDfy();
        }

        [Fact]
        public void ValidateLastName()
        {
            this.Given(_ => ChildWithLastName(NamePreValidation))
                .When(_ => ChildIsValidated())
                .Then(_ => LastNameIsFormatted(NamePostValidation))
                .WithExamples(Names.Valid)
                .BDDfy();
        }

        [Fact]
        public void InvalidFirstName()
        {
            this.Given(_ => ChildWithFirstName(NamePreValidation))
                .When(_ => ChildIsValidated())
                .Then(_ => ChildIsInvalid())
                .WithExamples(Names.Invalid)
                .BDDfy();
        }

        [Fact]
        public void InvalidLastName()
        {
            this.Given(_ => ChildWithLastName(NamePreValidation))
                .When(_ => ChildIsValidated())
                .Then(_ => ChildIsInvalid())
                .WithExamples(Names.Invalid)
                .BDDfy();
        }
    }
}