namespace Bubbio.Core.Repository
{
    /// <summary>
    /// Expose repository read and mutate operations.
    /// </summary>
    public interface IRepository : IReadRepository, IMutateRepository
    {
    }
}