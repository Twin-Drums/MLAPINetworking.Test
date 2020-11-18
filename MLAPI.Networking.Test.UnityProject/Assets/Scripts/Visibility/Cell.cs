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

        public event Action<CellClusterUpdate> onClusterUpdated = delegate { };

        public List<IVisibilityGridObject> Objects { get; private set; }
        public ulong Id { get; set; }

        protected NeighborCells neighbors;

        public Cell()
        {
            Objects = new List<IVisibilityGridObject>();
        }

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

        public virtual void SetUpNeighbors(Dictionary<ulong, ICell> grid)
        {
            SetUpNeighbors(this, grid);
        }

        public virtual void Reset()
        {
            this.Objects.Clear();
            CleanUpNeighborConnections(this);
            ClearNeighbors(this);
            onClusterUpdated = delegate { };
        }

        #region Cell Neighbors

        private struct NeighborConnector
        {
            public Cell.GridPosition Offset;
            public Action<Cell, Cell> Connector;

            public NeighborConnector(int offsetX, int offsetY, Action<Cell, Cell> connector)
            {
                Offset = new Cell.GridPosition { x = offsetX, y = offsetY };
                Connector = connector;
            }
        }

        private static readonly List<NeighborConnector> neighborConnectors = new List<NeighborConnector>
        {
            new NeighborConnector(0, 1, (origin, neighbor) => { origin.neighbors.Top = neighbor; neighbor.neighbors.Bottom = origin; }),
            new NeighborConnector(1, 1, (origin, neighbor) => { origin.neighbors.TopRight = neighbor; neighbor.neighbors.BottomLeft = origin; }),
            new NeighborConnector(1, 0, (origin, neighbor) => { origin.neighbors.Right = neighbor; neighbor.neighbors.Left = origin; }),
            new NeighborConnector(1, -1, (origin, neighbor) => { origin.neighbors.BottomRight = neighbor; neighbor.neighbors.TopLeft = origin; }),
            new NeighborConnector(0, -1, (origin, neighbor) => { origin.neighbors.Bottom = neighbor; neighbor.neighbors.Top = origin; }),
            new NeighborConnector(-1, -1, (origin, neighbor) => { origin.neighbors.BottomLeft = neighbor; neighbor.neighbors.TopRight = origin; }),
            new NeighborConnector(-1, 0, (origin, neighbor) => { origin.neighbors.Left = neighbor; neighbor.neighbors.Right = origin; }),
            new NeighborConnector(-1, 1, (origin, neighbor) => { origin.neighbors.TopLeft = neighbor; neighbor.neighbors.BottomRight = origin; }),
        };

        protected static void SetUpNeighbors(Cell cell, Dictionary<ulong, ICell> grid)
        {
            var gridPos = VisibilityGridHelper.GetGridPosition(cell.Id);

            for (int i = 0; i < neighborConnectors.Count; i++)
            {
                var nc = neighborConnectors[i];
                var neighborId = VisibilityGridHelper.GetCellId(gridPos.x + nc.Offset.x, gridPos.y + nc.Offset.y);

                ICell neighbor = null;
                if (!grid.TryGetValue(neighborId, out neighbor))
                    continue;

                nc.Connector(cell, neighbor as Cell);
            }
        }

        protected static void CleanUpNeighborConnections(Cell cell)
        {
            if (cell.neighbors.Top != null)
                cell.neighbors.Top.neighbors.Bottom = null;
            if (cell.neighbors.TopRight != null)
                cell.neighbors.TopRight.neighbors.BottomLeft = null;
            if (cell.neighbors.Right != null)
                cell.neighbors.Right.neighbors.Left = null;
            if (cell.neighbors.BottomRight != null)
                cell.neighbors.BottomRight.neighbors.TopLeft = null;
            if (cell.neighbors.Bottom != null)
                cell.neighbors.Bottom.neighbors.Top = null;
            if (cell.neighbors.BottomLeft != null)
                cell.neighbors.BottomLeft.neighbors.TopRight = null;
            if (cell.neighbors.Left != null)
                cell.neighbors.Left.neighbors.Right = null;
            if (cell.neighbors.TopLeft != null)
                cell.neighbors.TopLeft.neighbors.BottomRight = null;
        }

        protected static void ClearNeighbors(Cell cell)
        {
            cell.neighbors.Top = null;
            cell.neighbors.TopRight = null;
            cell.neighbors.Right = null;
            cell.neighbors.BottomRight = null;
            cell.neighbors.Bottom = null;
            cell.neighbors.BottomLeft = null;
            cell.neighbors.Left = null;
            cell.neighbors.TopLeft = null;
        }

        #endregion
    }
}

