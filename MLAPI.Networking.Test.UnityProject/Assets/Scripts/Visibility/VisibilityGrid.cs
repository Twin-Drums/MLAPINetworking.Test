using System;
using System.Collections.Generic;

namespace Twindrums.TheWagaduChronicles.Visibility
{
    public class VisibilityGrid<T> where T : ICell, new()
    {
        private float cellSize;
        private readonly Dictionary<ulong, ICell> grid = new Dictionary<ulong, ICell>();
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
                    continue;

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

            if (cell.Objects.Contains(gridObject))
                cell.Remove(gridObject);

            gridObject.Cell = null;

            if (cell.Objects.Count == 0)
            {
                RemoveCell(cell);
            }
        }

        private void RemoveCell(ICell cell)
        {
            grid.Remove(cell.Id);
            cell.Reset();
            ReturnListToPool(cell);
        }

        private ICell GetCell(ulong cellId)
        {
            ICell cell = null;

            if (grid.TryGetValue(cellId, out cell))
            {
                return cell;
            }

            cell = GetListFromPool();
            cell.Id = cellId;
            grid[cellId] = cell;
            cell.SetUpNeighbors(grid);
            return cell;
        }

        #region Cell Pool

        private T GetListFromPool()
        {
            return new T();// todo
        }

        private void ReturnListToPool(ICell cell)
        {
            cell.Reset();
            // todo
        }

        #endregion
        #region Cell Id

        public ulong GetCellId(float x, float y) => VisibilityGridHelper.GetCellId((int)(x / cellSize), (int)(y / cellSize));

        #endregion
    }

    public static class VisibilityGridHelper
    {
        public static ulong GetCellId(int gridX, int gridY) => (((ulong)(uint)gridX) << 32) | ((ulong)(uint)gridY);
        public static Cell.GridPosition GetGridPosition(ulong cellId) => new Cell.GridPosition { x = (int)(cellId >> 32), y = (int)cellId };
    }
}