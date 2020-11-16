using UnityEngine;
using System.Collections;
using MLAPI;
using System;
using Twindrums.TheWagaduChronicles.Visibility;

namespace Twindrums.TheWagaduChronicles.NetworkVisibility
{
    public class NetworkVisibilityObject : NetworkedBehaviour, IVisibilityGridObject
    {
        public static VisibilityGrid Grid
        {
            get
            {
                if(grid == null)
                {
                    grid = new VisibilityGrid(50, 10);
                }
                return grid;
            }
        }
        private static VisibilityGrid grid;


        public VisibilityGrid.Position Position =>  new VisibilityGrid.Position() { x = this.transform.position.x, y = this.transform.position.y };

        public VisibilityGrid.Cell Cell { get { return cell; } set { SetCellInternal(Cell); } }
        private VisibilityGrid.Cell cell;

        private void SetCellInternal(VisibilityGrid.Cell cell)
        {
            cell = Cell;
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            
        }

        public bool ShouldUpdate => true;

        public override void NetworkStart()
        {
            if (!IsServer)
                return;

            Grid.RegisterObject(this);
            NetworkedObject.CheckObjectVisibility += HandleCheckObjectVisibility;
        }

        private void OnDestroy()
        {
            if (!IsServer)
                return;

            NetworkedObject.CheckObjectVisibility -= HandleCheckObjectVisibility;
            Grid.UnregisterObject(this);
        }

        private bool HandleCheckObjectVisibility(ulong clientId)
        {
            return false;
        }        
    }
}