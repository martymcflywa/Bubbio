using System;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Core.Store;
using Bubbio.Store.MongoDb.Entities;

namespace Bubbio.Store.MongoDb.Contracts
{
    public interface IChild : IEntityCommon<Guid>
    {
        Guid ParentId { get; set; }
        Name Name { get; set; }
        DateTimeOffset DateOfBirth { get; set; }
        Gender Gender { get; set; }

        float InitialHeight { get; set; }
        float InitialWeight { get; set; }
        float InitialHeadCircumference { get; set; }
    }
}