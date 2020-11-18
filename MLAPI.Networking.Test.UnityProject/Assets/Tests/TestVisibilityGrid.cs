using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Twindrums.TheWagaduChronicles.Visibility;
using System;

namespace Tests
{
    public class TestVisibilityGrid
    {
        private class DummyVisibilityGridObject : IVisibilityGridObject
        {            
            public DummyVisibilityGridObject(float x, float y)
            {
                Position = new Cell.Position { x = x, y = y };
            }

            public Cell.Position Position { get; set; }

            public ICell Cell { get; set; }

            public bool ShouldUpdate => true;
        }

        private class TestCell : Cell
        {
            public NeighborCells Neighbors => neighbors;
        }

        [Test]
        public void CanConvertGridPositionToAndFromCellId()
        {            
            int gridX = 23563;
            int gridY = -985462;
            var id = VisibilityGridHelper.GetCellId(gridX, gridY);            
            var position = VisibilityGridHelper.GetGridPosition(id);
            Assert.AreEqual(gridX, position.x);
            Assert.AreEqual(gridY, position.y);

            gridX = -652546;
            gridY = 985462;
            id = VisibilityGridHelper.GetCellId(gridX, gridY);            
            position = VisibilityGridHelper.GetGridPosition(id);
            Assert.AreEqual(gridX, position.x);
            Assert.AreEqual(gridY, position.y);

            gridX = -2436;
            gridY = -234;
            id = VisibilityGridHelper.GetCellId(gridX, gridY);
            position = VisibilityGridHelper.GetGridPosition(id);
            Assert.AreEqual(gridX, position.x);
            Assert.AreEqual(gridY, position.y);
        }

        [Test]
        public void CanRegisterGridObject()
        {
            var grid = new VisibilityGrid<Cell>(1f);
            var gridObject = new DummyVisibilityGridObject(863567f, -6524565f);
            grid.RegisterObject(gridObject);

            Assert.NotNull(gridObject.Cell);
            var position = VisibilityGridHelper.GetGridPosition(gridObject.Cell.Id);
            Assert.AreEqual(gridObject.Position.x, position.x);
            Assert.AreEqual(gridObject.Position.y, position.y);
            Assert.Contains(gridObject, gridObject.Cell.Objects);
        }

        [Test]
        public void CanRemoveRegisteredGridObject()
        {
            var grid = new VisibilityGrid<Cell>(1);
            var gridObject = new DummyVisibilityGridObject(147654f, -65456f);
            grid.RegisterObject(gridObject);
            var cell = gridObject.Cell;
            grid.UnregisterObject(gridObject);
            Assert.IsNull(gridObject.Cell);
            Assert.IsFalse(cell.Objects.Contains(gridObject));
        }

        [Test]
        public void AddsCellNeighbors()
        {
            var objects = new List<DummyVisibilityGridObject>()
            {
                new DummyVisibilityGridObject(0,0),
                new DummyVisibilityGridObject(0,1),
                new DummyVisibilityGridObject(1,1),
                new DummyVisibilityGridObject(1,0),
                new DummyVisibilityGridObject(1,-1),
                new DummyVisibilityGridObject(0,-1),
                new DummyVisibilityGridObject(-1,-1),
                new DummyVisibilityGridObject(-1,0),
                new DummyVisibilityGridObject(-1,1),
            };

            var grid = new VisibilityGrid<TestCell>(1);

            foreach (var item in objects)
            {
                grid.RegisterObject(item);
            }

            foreach (var item in objects)
                Assert.AreEqual(item.Cell.Objects.Count, 1);
            
            Assert.AreSame((objects[0].Cell as TestCell).Neighbors.Top, objects[1].Cell);
            Assert.AreSame((objects[1].Cell as TestCell).Neighbors.Bottom, objects[0].Cell);
            Assert.AreSame((objects[0].Cell as TestCell).Neighbors.TopRight, objects[2].Cell);
            Assert.AreSame((objects[2].Cell as TestCell).Neighbors.BottomLeft, objects[0].Cell);
            Assert.AreSame((objects[0].Cell as TestCell).Neighbors.Right, objects[3].Cell);
            Assert.AreSame((objects[3].Cell as TestCell).Neighbors.Left, objects[0].Cell);
            Assert.AreSame((objects[0].Cell as TestCell).Neighbors.BottomRight, objects[4].Cell);
            Assert.AreSame((objects[4].Cell as TestCell).Neighbors.TopLeft, objects[0].Cell);
        }

        [Test]
        public void RemovesCellNeighbors()
        {
            var objects = new List<DummyVisibilityGridObject>()
            {
                new DummyVisibilityGridObject(0,0),
                new DummyVisibilityGridObject(0,1),
                new DummyVisibilityGridObject(1,1),
                new DummyVisibilityGridObject(1,0),
                new DummyVisibilityGridObject(1,-1),
                new DummyVisibilityGridObject(0,-1),
                new DummyVisibilityGridObject(-1,-1),
                new DummyVisibilityGridObject(-1,0),
                new DummyVisibilityGridObject(-1,1),
            };

            var grid = new VisibilityGrid<TestCell>(1);

            foreach (var item in objects)
            {
                grid.RegisterObject(item);
            }

            foreach (var item in objects)
                Assert.AreEqual(item.Cell.Objects.Count, 1);

            grid.UnregisterObject(objects[0]);
            
            Assert.IsNull((objects[1].Cell as TestCell).Neighbors.Bottom);
            Assert.IsNull((objects[2].Cell as TestCell).Neighbors.BottomLeft);
            Assert.IsNull((objects[3].Cell as TestCell).Neighbors.Left);
            Assert.IsNull((objects[4].Cell as TestCell).Neighbors.TopLeft);
        }
    }
}
