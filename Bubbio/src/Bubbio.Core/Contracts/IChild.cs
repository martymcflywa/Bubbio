﻿using System;
using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Core.Contracts
{
    public interface IChild
    {
        Guid ParentId { get; set; }

        IName Name { get; set; }
        DateTimeOffset DateOfBirth { get; set; }
        Gender Gender { get; set; }

        float InitialHeight { get; set; }
        float InitialWeight { get; set; }
        float InitialHeadCircumference { get; set; }
    }
}