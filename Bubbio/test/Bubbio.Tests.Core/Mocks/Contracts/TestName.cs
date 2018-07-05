using Bubbio.Core.Contracts;

namespace Bubbio.Tests.Core.Mocks.Contracts
{
    public class TestName : IName
    {
        public string First { get; set; }
        public string Middle { get; set; } = string.Empty;
        public string Last { get; set; }

        public override string ToString()
        {
            return $"{First} {Middle} {Last}";
        }
    }
}