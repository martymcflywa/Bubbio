﻿namespace Bubbio.Core.Store
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}