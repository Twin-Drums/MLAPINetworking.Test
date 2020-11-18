using UnityEngine;
using System.Collections;
using MLAPI;
using System;
using Twindrums.TheWagaduChronicles.Visibility;
using System.Collections.Generic;
using Twindrums.TheWagaduChronicles.NPC;
using MLAPI.Prototyping;
using UnityEngine.AI;

namespace Twindrums.TheWagaduChronicles.NetworkVisibility
{    
    public class NPCNetworkVisibilityObject: NetworkVisibilityObject
    {
        public override bool ShouldUpdate => base.ShouldUpdate && shouldUpdate;
        private bool shouldUpdate = true;        

        protected override void HandleCellChanged(ICell oldCell, ICell newCell)
        {
            base.HandleCellChanged(oldCell, newCell);

            if (oldCell != null)
                (oldCell as NetworkedVisibility.NetworkCell).onPlayersUpdated -= HandlePlayersUpdated;

            if (newCell == null)
                return;

            (newCell as NetworkedVisibility.NetworkCell).onPlayersUpdated += HandlePlayersUpdated;
            UpdateEnabledState(newCell);            
        }

        private void HandlePlayersUpdated(Cell.CellClusterUpdate update)
        {            
            if (Cell == null)
                return;

            UpdateEnabledState(Cell);
        }

        private void UpdateEnabledState(ICell cell)
        {
            var networkCell = cell as NetworkedVisibility.NetworkCell;
            bool hasPlayers = networkCell.Players.Count > 0;

            if (gameObject.activeSelf && !hasPlayers)
            {
                gameObject.SetActive(false);                
                shouldUpdate = false;
            }
            else if (!gameObject.activeSelf && hasPlayers)
            {
                gameObject.SetActive(true);                
                shouldUpdate = true;
            }
        }
    }
}