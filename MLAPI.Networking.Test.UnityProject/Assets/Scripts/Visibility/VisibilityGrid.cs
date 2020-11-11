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

        private ushort cellSize;
        private Dictionary<ulong, List<IVisibilityGridObject>> grid = new Dictionary<ulong, List<IVisibilityGridObject>>();

        public VisibilityGrid(ushort cellSize, ushort initialAmountOfCellLists = 10)
        {
            this.cellSize = cellSize;
        }

        public ulong RegisterObject(IVisibilityGridObject gridObject)
        {
            var pos = gridObject.Position;
            ulong cellId = GetCellId(pos.x, pos.y);
            gridObject.CellId = cellId;
            AddToList(gridObject);
            return cellId;
        }

        public void UnregisterObject(IVisibilityGridObject gridObject)
        {
            RemoveFromList(gridObject);
        }

        private void AddToList(IVisibilityGridObject gridObject)
        {
            GetList(gridObject.CellId).Add(gridObject);
        }

        private void RemoveFromList(IVisibilityGridObject gridObject)
        {
            var list = GetList(gridObject.CellId);
            list.Remove(gridObject);
            if(list.Count == 0)
            {
                grid.Remove(gridObject.CellId);
                ReturnListToPool(list);
            }
        }

        private List<IVisibilityGridObject> GetList(ulong cellId)
        {
            List<IVisibilityGridObject> list = null;

            if(grid.TryGetValue(cellId, out list))
            {
                return list;
            }

            list = GetListFromPool();
            grid[cellId] = list;
            return list;
        }

        private List<IVisibilityGridObject> GetListFromPool()
        {
            return new List<IVisibilityGridObject>();
        }

        private void ReturnListToPool(List<IVisibilityGridObject> list)
        {
            
        }

        private ulong GetCellId(float x, float y) => ((ulong)(x / cellSize) << 32) | (ulong)(y / cellSize);
    }
}