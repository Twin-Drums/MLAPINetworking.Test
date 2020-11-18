using System;
using System.Collections.Generic;

namespace Twindrums.TheWagaduChronicles.Visibility
{
    public interface ICell
    {
        event Action<Cell.CellClusterUpdate> onClusterUpdated;
        void Add(IVisibilityGridObject gridObject);
        void Broadcast(Cell.CellClusterUpdate update);
        void Remove(IVisibilityGridObject gridObject);
        void Reset();
    }
}