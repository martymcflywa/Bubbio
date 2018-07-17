namespace Bubbio.Repository.Core.Interfaces
{
    /// <summary>
    /// Expose repository read and mutate operations.
    /// </summary>
    public interface IRepository : IReadRepository, IMutateRepository
    {
    }
}