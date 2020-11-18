using System;
using System.Collections.Generic;

namespace Twindrums.TheWagaduChronicles.Visibility
{
    public class VisibilityGrid
    {
        private float cellSize;
        private readonly Dictionary<ulong, Cell> grid = new Dictionary<ulong, Cell>();
        private readonly List<IVisibilityGridObject> objects = new List<IVisibilityGridObject>();

        public VisibilityGrid(float cellSize, ushort initialAmountOfCellLists = 10)
        {
            this.cellSize = cellSize;
        }

        public void RegisterObject(IVisibilityGridObject gridObject)
        {
            if (objects.Contains(gridObject))
                return;

            objects.Add(gridObject);            
            AddToCell(GetCellId(gridObject.Position.x, gridObject.Position.y), gridObject);            
            return;
        }

        public void UnregisterObject(IVisibilityGridObject gridObject)
        {
            if (objects.Contains(gridObject))
                objects.Remove(gridObject);

            if (gridObject.Cell == null)
                return;
            RemoveFromCell(gridObject);
        }

        public void Update()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                var gridObject = objects[i];

                if (!gridObject.ShouldUpdate)
                    return;

                ulong cellId = GetCellId(gridObject.Position.x, gridObject.Position.y);
                if (gridObject.Cell.Id == cellId)
                    continue;
                RemoveFromCell(gridObject);
                AddToCell(cellId, gridObject);                
            }
        }

        private void AddToCell(ulong cellId, IVisibilityGridObject gridObject)
        {
            var cell = GetCell(cellId);
            cell.Add(gridObject);            
            gridObject.Cell = cell;
        }

        private void RemoveFromCell(IVisibilityGridObject gridObject)
        {
            var cell = gridObject.Cell;

            if(cell.Objects.Contains(gridObject))
                cell.Remove(gridObject);

            gridObject.Cell = null;

            if(cell.Objects.Count == 0)
            {
                RemoveCell(cell);
            }
        }

        private void RemoveCell(Cell cell)
        {
            grid.Remove(cell.Id);
            CleanUpNeighbors(cell);
            ReturnListToPool(cell);
        }

        private Cell GetCell(ulong cellId)
        {
            Cell cell = null;

            if(grid.TryGetValue(cellId, out cell))
            {
                return cell;
            }

            cell = GetListFromPool();
            cell.Id = cellId;
            grid[cellId] = cell;
            SetUpNeighbors(cell);
            return cell;
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

        private readonly List<NeighborConnector> neighborConnectors = new List<NeighborConnector>
        {
            new NeighborConnector(0, 1, (origin, neighbor) => { origin.Neighbors.Top = neighbor; neighbor.Neighbors.Bottom = origin; }),
            new NeighborConnector(1, 1, (origin, neighbor) => { origin.Neighbors.TopRight = neighbor; neighbor.Neighbors.BottomLeft = origin; }),
            new NeighborConnector(1, 0, (origin, neighbor) => { origin.Neighbors.Right = neighbor; neighbor.Neighbors.Left = origin; }),
            new NeighborConnector(1, -1, (origin, neighbor) => { origin.Neighbors.BottomRight = neighbor; neighbor.Neighbors.TopLeft = origin; }),
            new NeighborConnector(0, -1, (origin, neighbor) => { origin.Neighbors.Bottom = neighbor; neighbor.Neighbors.Top = origin; }),
            new NeighborConnector(-1, -1, (origin, neighbor) => { origin.Neighbors.BottomLeft = neighbor; neighbor.Neighbors.TopRight = origin; }),
            new NeighborConnector(-1, 0, (origin, neighbor) => { origin.Neighbors.Left = neighbor; neighbor.Neighbors.Right = origin; }),
            new NeighborConnector(-1, 1, (origin, neighbor) => { origin.Neighbors.TopLeft = neighbor; neighbor.Neighbors.BottomRight = origin; }),            
        };

        private void SetUpNeighbors(Cell cell)
        {
            var gridPos = GetGridPosition(cell.Id);

            for (int i = 0; i < neighborConnectors.Count; i++)
            {
                var nc = neighborConnectors[i];
                var neighborId = GetCellId(gridPos.x + nc.Offset.x, gridPos.y + nc.Offset.y);

                Cell neighbor = null;
                if (!grid.TryGetValue(neighborId, out neighbor))
                    continue;

                nc.Connector(cell, neighbor);
            }
        }

        private void CleanUpNeighbors(Cell cell)
        {
            if (cell.Neighbors.Top != null)
                cell.Neighbors.Top.Neighbors.Bottom = null;
            if (cell.Neighbors.TopRight != null)
                cell.Neighbors.TopRight.Neighbors.BottomLeft = null;
            if (cell.Neighbors.Right != null)
                cell.Neighbors.Right.Neighbors.Left = null;
            if (cell.Neighbors.BottomRight != null)
                cell.Neighbors.BottomRight.Neighbors.TopLeft = null;
            if (cell.Neighbors.Bottom != null)
                cell.Neighbors.Bottom.Neighbors.Top = null;
            if (cell.Neighbors.BottomLeft != null)
                cell.Neighbors.BottomLeft.Neighbors.TopRight = null;
            if (cell.Neighbors.Left != null)
                cell.Neighbors.Left.Neighbors.Right = null;
            if (cell.Neighbors.TopLeft != null)
                cell.Neighbors.TopLeft.Neighbors.BottomRight = null;
        }

        #endregion
        #region Cell Pool

        private Cell GetListFromPool()
        {
            return new Cell() { Objects = new List<IVisibilityGridObject>() };// todo
        }

        private void ReturnListToPool(Cell cell)
        {
            cell.Reset();
            // todo
        }

        #endregion
        #region Cell Id

        public ulong GetCellId(float x, float y) => GetCellId((int)(x / cellSize), (int)(y / cellSize));
        public static ulong GetCellId(int gridX, int gridY) => (((ulong)(uint)gridX) << 32) | ((ulong)(uint)gridY);
        public static Cell.GridPosition GetGridPosition(ulong cellId) => new Cell.GridPosition { x = (int)(cellId >> 32), y = (int)cellId };

        #endregion
    }
}