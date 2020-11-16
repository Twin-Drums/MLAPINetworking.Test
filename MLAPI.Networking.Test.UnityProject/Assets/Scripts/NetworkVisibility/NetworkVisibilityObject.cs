using UnityEngine;
using System.Collections;
using MLAPI;
using System;
using Twindrums.TheWagaduChronicles.Visibility;

namespace Twindrums.TheWagaduChronicles.NetworkVisibility
{
    public class NetworkVisibilityObject : NetworkedBehaviour, IVisibilityGridObject
    {
        public VisibilityGrid.Position Position =>  new VisibilityGrid.Position() { x = this.transform.position.x, y = this.transform.position.y };

        public VisibilityGrid.Cell Cell { get { return cell; } set { SetCellInternal(value); } }
        private VisibilityGrid.Cell cell;

        private void SetCellInternal(VisibilityGrid.Cell cell)
        {
            var oldCell = cell;
            this.cell = cell;
            UpdateVisibility(oldCell, cell);
        }

        private void UpdateVisibility(VisibilityGrid.Cell oldCell, VisibilityGrid.Cell cell)
        {
            
        }

        public bool ShouldUpdate => true;

        public override void NetworkStart()
        {
            if (!IsServer)
                return;

            NetworkedVisibility.Grid.RegisterObject(this);
            NetworkedObject.CheckObjectVisibility += HandleCheckObjectVisibility;
        }

        private void OnDestroy()
        {
            if (!IsServer)
                return;

            NetworkedObject.CheckObjectVisibility -= HandleCheckObjectVisibility;
            NetworkedVisibility.Grid.UnregisterObject(this);
        }

        private bool HandleCheckObjectVisibility(ulong clientId)
        {
            return false;
        }        
    }
}