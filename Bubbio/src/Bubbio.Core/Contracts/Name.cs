namespace Bubbio.Core.Contracts
{
    public class Name : IName
    {
        public string First { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }

        public override string ToString()
        {
            return $"{First} {Middle} {Last}";
        }
    }
}