using System;
using System.Collections.Generic;

namespace Twindrums.TheWagaduChronicles.Visibility
{
    public class VisibilityGrid
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

        public class Cell
        {            
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

            public List<IVisibilityGridObject> Objects;
            public ulong Id;
            public NeighborCells Neighbors;
        }

        private float cellSize;
        private Dictionary<ulong, Cell> grid = new Dictionary<ulong, Cell>();

        public VisibilityGrid(float cellSize, ushort initialAmountOfCellLists = 10)
        {
            this.cellSize = cellSize;
        }

        public ulong RegisterObject(IVisibilityGridObject gridObject)
        {
            var pos = gridObject.Position;
            ulong cellId = GetCellId(pos.x, pos.y);            
            AddToCell(cellId, gridObject);
            return cellId;
        }

        public void UnregisterObject(IVisibilityGridObject gridObject)
        {
            if (gridObject.Cell == null)
                return;
            RemoveFromCell(gridObject);
        }

        private void AddToCell(ulong cellId, IVisibilityGridObject gridObject)
        {
            var cell = GetCell(cellId);
            cell.Objects.Add(gridObject);
            gridObject.Cell = cell;
        }

        private void RemoveFromCell(IVisibilityGridObject gridObject)
        {
            var cell = gridObject.Cell;

            if(cell.Objects.Contains(gridObject))
                cell.Objects.Remove(gridObject);

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

        private void CleanUpNeighbors(Cell cell)
        {
            // todo
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

        private struct NeighborConnector
        {
            public GridPosition Offset;
            public Action<Cell, Cell> Connector;

            public NeighborConnector(int offsetX, int offsetY, Action<Cell, Cell> connector)
            {
                Offset = new GridPosition { x = offsetX, y = offsetY };
                Connector = connector;
            }
        }

        private readonly List<NeighborConnector> neighborConnectors = new List<NeighborConnector>
        {
            
        };

        private void SetUpNeighbors(Cell cell)
        {
            var gridPos = GetGridPosition(cell.Id);

        }

        private Cell GetListFromPool()
        {
            return new Cell() { Objects = new List<IVisibilityGridObject>() };// todo
        }

        private void ReturnListToPool(Cell cell)
        {
            // todo
        }

        public ulong GetCellId(float x, float y) => GetCellId((int)(x / cellSize), (int)(y / cellSize));
        public static ulong GetCellId(int gridX, int gridY) => (((ulong)(uint)gridX) << 32) | ((ulong)(uint)gridY);
        public static GridPosition GetGridPosition(ulong cellId) => new GridPosition { x = (int)(cellId >> 32), y = (int)cellId };
    }
}