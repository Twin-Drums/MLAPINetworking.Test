using System;
using System.Collections.Generic;

namespace Twindrums.TheWagaduChronicles.Visibility
{
    public interface ICell
    {
        event Action<Cell.CellClusterUpdate> onClusterUpdated;

        ulong Id { get; set; }
        List<IVisibilityGridObject> Objects { get;  }

        void SetUpNeighbors(Dictionary<ulong, ICell> grid);
        void Add(IVisibilityGridObject gridObject);
        void Broadcast(Cell.CellClusterUpdate update);
        void Remove(IVisibilityGridObject gridObject);
        void Reset();
    }
}