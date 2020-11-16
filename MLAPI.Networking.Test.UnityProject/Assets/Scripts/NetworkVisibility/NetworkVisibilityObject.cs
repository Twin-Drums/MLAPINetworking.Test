using UnityEngine;
using System.Collections;
using MLAPI;
using System;
using Twindrums.TheWagaduChronicles.Visibility;
using System.Collections.Generic;
using Twindrums.TheWagaduChronicles.Player;

namespace Twindrums.TheWagaduChronicles.NetworkVisibility
{
    public class NetworkVisibilityObject : NetworkedBehaviour, IVisibilityGridObject
    {
        public VisibilityGrid.Position Position =>  new VisibilityGrid.Position() { x = this.transform.position.x, y = this.transform.position.z };

        public VisibilityGrid.Cell Cell { get { return cell; } set { SetCellInternal(value); } }
        private VisibilityGrid.Cell cell;

        public bool IsPlayer;

        private void SetCellInternal(VisibilityGrid.Cell cell)
        {
            var oldCell = cell;
            this.cell = cell;
            UpdateVisibility(oldCell, cell);
        }

        private void UpdateVisibility(VisibilityGrid.Cell oldCell, VisibilityGrid.Cell cell)
        {
            if(oldCell != null)
            {
                oldCell.onClusterUpdated -= HandleClusterUpdated;
                RemovePlayerVisibility(oldCell.Objects);
            }

            if (cell == null)
                return;

            AddPlayerVisibility(cell.Objects);

            cell.onClusterUpdated += HandleClusterUpdated;

        }

        private void RemovePlayerVisibility(List<IVisibilityGridObject> objects)
        {
            foreach (var item in objects)
            {
                RemovePlayerVisibility(item);
            }
        }

        private void RemovePlayerVisibility(IVisibilityGridObject gridObject)
        {
            if (!(gridObject is NetworkVisibilityObject))
                return;

            var nvo = gridObject as NetworkVisibilityObject;

            if (!nvo.IsPlayer)
                return;

            if (this.OwnerClientId == nvo.OwnerClientId)
                return;

            if (this.NetworkedObject.IsNetworkVisibleTo(nvo.NetworkedObject.OwnerClientId))
                this.NetworkedObject.NetworkHide(nvo.NetworkedObject.OwnerClientId);
        }

        private void AddPlayerVisibility(List<IVisibilityGridObject> objects)
        {
            foreach (var item in objects)
            {
                AddPlayerVisibility(item);
            }
        }

        private void AddPlayerVisibility(IVisibilityGridObject gridObject)
        {
            if (!(gridObject is NetworkVisibilityObject))
                return;

            var nvo = gridObject as NetworkVisibilityObject;

            if (this.OwnerClientId == nvo.OwnerClientId)
                return;

            if (!nvo.IsPlayer)
                return;

            if (!this.NetworkedObject.IsNetworkVisibleTo(nvo.NetworkedObject.OwnerClientId))
                this.NetworkedObject.NetworkShow(nvo.NetworkedObject.OwnerClientId);
        }

        private void HandleClusterUpdated(VisibilityGrid.CellClusterUpdate clusterUpdate)
        {
            switch (clusterUpdate.Type)
            {
                case VisibilityGrid.CellClusterUpdate.UpdateType.Added:
                    AddPlayerVisibility(clusterUpdate.Object);
                    break;
                case VisibilityGrid.CellClusterUpdate.UpdateType.Removed:
                    RemovePlayerVisibility(clusterUpdate.Object);
                    break;
            }
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
            return IsPlayer && (this.NetworkedObject.OwnerClientId == clientId);
        }
    }
}