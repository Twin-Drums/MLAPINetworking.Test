using System;
using System.Collections.Generic;

namespace Twindrums.TheWagaduChronicles.Visibility
{
    public class Cell : ICell
    {
        public struct Position
        {
            public float x;
            public float y;
        }

        public struct GridPosition
        {
            public int x;
            public int y;
        }

        public struct NeighborCells
        {
            public Cell Top;
            public Cell TopRight;
            public Cell Right;
            public Cell BottomRight;
            public Cell Bottom;
            public Cell BottomLeft;
            public Cell Left;
            public Cell TopLeft;
        }

        public event Action<CellClusterUpdate> onClusterUpdated = delegate { };

        public List<IVisibilityGridObject> Objects;
        public ulong Id;
        public NeighborCells Neighbors;

        public virtual void Add(IVisibilityGridObject gridObject)
        {
            Objects.Add(gridObject);
            Broadcast(new CellClusterUpdate(CellClusterUpdate.UpdateType.Added, gridObject));
        }

        public virtual void Remove(IVisibilityGridObject gridObject)
        {
            Objects.Remove(gridObject);
            Broadcast(new CellClusterUpdate(CellClusterUpdate.UpdateType.Removed, gridObject));
        }

        public virtual void Broadcast(CellClusterUpdate update)
        {
            this.onClusterUpdated(update);
        }

        public virtual void Reset()
        {
            this.Objects.Clear();
            Neighbors.Top = null;
            Neighbors.TopRight = null;
            Neighbors.Right = null;
            Neighbors.BottomRight = null;
            Neighbors.Bottom = null;
            Neighbors.BottomLeft = null;
            Neighbors.Left = null;
            Neighbors.TopLeft = null;
            onClusterUpdated = delegate { };
        }

        public struct CellClusterUpdate
        {
            public enum UpdateType { Added, Removed }

            public UpdateType Type;
            public IVisibilityGridObject Object;

            public CellClusterUpdate(UpdateType Type, IVisibilityGridObject gridObject)
            {
                this.Type = Type;
                this.Object = gridObject;
            }
        }
    }
}

