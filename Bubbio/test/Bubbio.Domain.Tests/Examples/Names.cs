using TestStack.BDDfy;

namespace Bubbio.Domain.Tests.Examples
{
    public static class Names
    {
        private const string Pre = "Name Pre Validation";
        private const string Post = "Name Post Validation";

        public static ExampleTable Valid => new ExampleTable(Pre, Post)
        {
            {"lowercase", "Lowercase"},
            {"UPPERCASE", "Uppercase"},
            {"lowercase-hyphen", "Lowercase-Hyphen"},
            {"mclowercase", "McLowercase"},
            {"d'arcy", "D'Arcy"},
            {"lee'", "Lee"},
            {"name with spaces", "Name With Spaces"}
        };

        public static ExampleTable Invalid => new ExampleTable(Pre, Post)
        {
            {null, null},
            {"", null},
            {" ", null},
            {"1234", null},
            {"L33t", null},
            {"Sn**p", null}
        };
    }
}