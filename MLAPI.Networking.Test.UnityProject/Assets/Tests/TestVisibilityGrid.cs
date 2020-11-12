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
            public VisibilityGrid.Position Position { get; set; }

            public VisibilityGrid.Cell Cell { get; set; }
        }

        [Test]
        public void CanConvertGridPositionToAndFromCellId()
        {            
            int gridX = 23563;
            int gridY = -985462;
            var id = VisibilityGrid.GetCellId(gridX, gridY);
            Debug.LogFormat("[TestVisibilityGrid::CanConvertGridPositionToAndFromCellId] gridX={0} gridY={1} id={2}", gridX, gridY, id);
            var position = VisibilityGrid.GetGridPosition(id);
            Assert.AreEqual(gridX, position.x);
            Assert.AreEqual(gridY, position.y);
        }
        
        [Test]
        public void CanRegisterGridObject()
        {
            var grid = new VisibilityGrid(1f);
            var gridObject = new DummyVisibilityGridObject();
            gridObject.Position = new VisibilityGrid.Position { x = 863567f, y = -6524565f };            
            grid.RegisterObject(gridObject);

            Assert.NotNull(gridObject.Cell);
            var position = VisibilityGrid.GetGridPosition(gridObject.Cell.Id);
            Assert.AreEqual(gridObject.Position.x, position.x);
            Assert.AreEqual(gridObject.Position.y, position.y);
            Assert.Contains(gridObject, gridObject.Cell.Objects);
        }

        [Test]
        public void CanRemoveRegisteredGridObject()
        {
            var grid = new VisibilityGrid(1);
            var gridObject = new DummyVisibilityGridObject();
            gridObject.Position = new VisibilityGrid.Position { x = 147654, y = 65456 };
            grid.RegisterObject(gridObject);
            var cell = gridObject.Cell;
            grid.UnregisterObject(gridObject);
            Assert.IsNull(gridObject.Cell);
            Assert.IsFalse(cell.Objects.Contains(gridObject));
        }
    }
}
