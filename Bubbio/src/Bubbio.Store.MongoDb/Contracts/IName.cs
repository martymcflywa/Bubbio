namespace Bubbio.Store.MongoDb.Contracts
{
    public interface IName
    {
        string First { get; set; }
        string Middle { get; set; }
        string Last { get; set; }

        string ToString();
    }
}