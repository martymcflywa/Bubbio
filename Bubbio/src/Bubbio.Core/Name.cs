using Bubbio.Core.Contracts;

namespace Bubbio.Core
{
    public class Name : IName
    {
        public string First { get; set; }
        public string Middle { get; set; } = string.Empty;
        public string Last { get; set; }
    }
}