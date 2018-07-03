using Bubbio.Domain.Tests.Examples;
using Bubbio.Domain.Tests.Scenarios;
using TestStack.BDDfy;
using Xunit;

namespace Bubbio.Domain.Tests.Validators
{
    public class ParentExTests : ParentExTestsBase
    {
        [Fact]
        public void ValidateFirstName()
        {
            this.Given(_ => ParentWithFirstName(NamePreValidation))
                .When(_ => ParentIsValidated())
                .Then(_ => FirstNameIsFormatted(NamePostValidation))
                .WithExamples(Names.Valid)
                .BDDfy();
        }

        [Fact]
        public void ValidateMiddleName()
        {
            this.Given(_ => ParentWithMiddleName(NamePreValidation))
                .When(_ => ParentIsValidated())
                .Then(_ => MiddleNameIsFormatted(NamePostValidation))
                .WithExamples(Names.Valid)
                .BDDfy();
        }

        [Fact]
        public void ValidateLastName()
        {
            this.Given(_ => ParentWithLastName(NamePreValidation))
                .When(_ => ParentIsValidated())
                .Then(_ => LastNameIsFormatted(NamePostValidation))
                .WithExamples(Names.Valid)
                .BDDfy();
        }

        [Fact]
        public void InvalidFirstName()
        {
            this.Given(_ => ParentWithFirstName(NamePreValidation))
                .When(_ => ParentIsValidated())
                .Then(_ => ParentIsInvalid())
                .WithExamples(Names.Invalid)
                .BDDfy();
        }

        [Fact]
        public void InvalidLastName()
        {
            this.Given(_ => ParentWithLastName(NamePreValidation))
                .When(_ => ParentIsValidated())
                .Then(_ => ParentIsInvalid())
                .WithExamples(Names.Invalid)
                .BDDfy();
        }

        [Fact]
        public void WithoutId()
        {
            this.Given(_ => ParentWithoutId())
                .When(_ => ParentIsValidated())
                .Then(_ => ParentIsInvalid())
                .BDDfy();
        }
    }
}