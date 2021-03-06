﻿using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Core.Contracts.Events
{
    public interface IBreastFeed : ITransitionEvent
    {
        Side Side { get; set; }
    }
}