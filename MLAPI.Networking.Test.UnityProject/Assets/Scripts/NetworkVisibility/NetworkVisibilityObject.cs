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
        public Cell.Position Position =>  new Cell.Position() { x = this.transform.position.x, y = this.transform.position.z };

        public ICell Cell { get { return cell; } set { SetCellInternal(value); } }
        protected ICell cell;

        public bool IsPlayer;

        protected virtual void SetCellInternal(ICell newCell)
        {
            var oldCell = this.cell;
            this.cell = newCell;
            HandleCellChanged(oldCell, newCell);
        }

        protected virtual void HandleCellChanged(ICell oldCell, ICell newCell)
        {
            if(oldCell != null)
            {
                oldCell.onClusterUpdated -= HandleClusterUpdated;
                RemovePlayerVisibility(oldCell.Objects);
                if (IsPlayer)
                    RemovePlayer(oldCell);
            }

            if (newCell == null)
                return;

            AddPlayerVisibility(newCell.Objects);
            if (IsPlayer)
                AddPlayer(newCell);

            newCell.onClusterUpdated += HandleClusterUpdated;
        }

        protected virtual void RemovePlayer(ICell cell)
        {
            var networkCell = cell as NetworkedVisibility.NetworkCell;
            networkCell.RemovePlayer(this);
        }

        protected virtual void AddPlayer(ICell cell)
        {
            var networkCell = cell as NetworkedVisibility.NetworkCell;
            networkCell.AddPlayer(this);
        }

        protected virtual void RemovePlayerVisibility(List<IVisibilityGridObject> objects)
        {
            foreach (var item in objects)
            {
                RemovePlayerVisibility(item);
            }
        }

        protected virtual void RemovePlayerVisibility(IVisibilityGridObject gridObject)
        {
            if (!(gridObject is NetworkVisibilityObject))
                return;

            var nvo = gridObject as NetworkVisibilityObject;

            if (nvo.OwnerClientId == 0)//Server
                return;

            if (!IsPlayer && !nvo.IsPlayer)
                return;

            if (this.OwnerClientId == nvo.OwnerClientId)
                return;

            if (this.NetworkedObject.IsNetworkVisibleTo(nvo.NetworkedObject.OwnerClientId))
                this.NetworkedObject.NetworkHide(nvo.NetworkedObject.OwnerClientId);
        }

        protected virtual void AddPlayerVisibility(List<IVisibilityGridObject> objects)
        {
            foreach (var item in objects)
            {
                AddPlayerVisibility(item);
            }
        }

        protected virtual void AddPlayerVisibility(IVisibilityGridObject gridObject)
        {
            if (!(gridObject is NetworkVisibilityObject))
                return;

            var nvo = gridObject as NetworkVisibilityObject;

            if (nvo.OwnerClientId == 0)//Server
                return;

            if (this.OwnerClientId == nvo.OwnerClientId)
                return;

            if (!IsPlayer && !nvo.IsPlayer)
                return;

            if (!this.NetworkedObject.IsNetworkVisibleTo(nvo.NetworkedObject.OwnerClientId))
                this.NetworkedObject.NetworkShow(nvo.NetworkedObject.OwnerClientId);
        }

        protected virtual void HandleClusterUpdated(Cell.CellClusterUpdate clusterUpdate)
        {
            switch (clusterUpdate.Type)
            {
                case  Visibility.Cell.CellClusterUpdate.UpdateType.Added:
                    AddPlayerVisibility(clusterUpdate.Object);
                    break;
                case Visibility.Cell.CellClusterUpdate.UpdateType.Removed:
                    RemovePlayerVisibility(clusterUpdate.Object);
                    break;
            }
        }

        public virtual bool ShouldUpdate => true;

        public override void NetworkStart()
        {
            if (!IsServer)
                return;

            NetworkedVisibility.Grid.RegisterObject(this);
            NetworkedObject.CheckObjectVisibility += HandleCheckObjectVisibility;
        }

        protected virtual void OnDestroy()
        {
            if (!IsServer)
                return;

            NetworkedObject.CheckObjectVisibility -= HandleCheckObjectVisibility;
            NetworkedVisibility.Grid.UnregisterObject(this);
        }

        protected virtual bool HandleCheckObjectVisibility(ulong clientId)
        {
            return IsPlayer && (this.NetworkedObject.OwnerClientId == clientId);
        }
    }
}