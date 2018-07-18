namespace Bubbio.MongoDb.Interfaces
{
    /// <summary>
    /// Expose repository read and mutate operations.
    /// </summary>
    public interface IMongoDbRepository : IReadMongoDbRepository, IMutateMongoDbRepository
    {
    }
}