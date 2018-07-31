using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Core.Contracts
{
    public interface IParent
    {
        IName Name { get; set; }
        MeasureType MeasureType { get; set; }
    }
}