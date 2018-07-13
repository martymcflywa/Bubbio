namespace Bubbio.Store.MongoDb.Abstractions
{
    /// <inheritdoc cref="IReadRepository" />
    /// <inheritdoc cref="IMutateRepository" />
    /// <inheritdoc cref="IRepositoryHelper" />
    /// <summary>
    /// Interface for repository operations.
    /// </summary>
    public interface IMongoDbRepository :
        IReadRepository,
        IMutateRepository,
        IRepositoryHelper
    {
    }
}