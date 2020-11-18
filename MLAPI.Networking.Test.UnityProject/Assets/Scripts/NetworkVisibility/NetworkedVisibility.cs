using UnityEngine;
using System.Collections;
using MLAPI;
using System;
using Twindrums.TheWagaduChronicles.Visibility;
using System.Collections.Generic;

namespace Twindrums.TheWagaduChronicles.NetworkVisibility
{
    public class NetworkedVisibility : NetworkedBehaviour
    {
        public class NetworkCell : Cell
        {
            public event Action<Cell.CellClusterUpdate> onPlayersUpdated = delegate { };

            public List<NetworkVisibilityObject> Players { get; private set; }

            public NetworkCell()
            {
                Players = new List<NetworkVisibilityObject>();
            }

            public override void Reset()
            {
                base.Reset();
                Players.Clear();
            }

            public void AddPlayer(NetworkVisibilityObject player)
            {
                if (Players.Contains(player))
                    return;

                Players.Add(player);
                onPlayersUpdated.Invoke(new CellClusterUpdate { Object = player, Type = CellClusterUpdate.UpdateType.Added });
            }

            public void RemovePlayer(NetworkVisibilityObject player)
            {
                if (!Players.Contains(player))
                    return;

                Players.Remove(player);
                onPlayersUpdated.Invoke(new CellClusterUpdate { Object = player, Type = CellClusterUpdate.UpdateType.Removed });
            }
        }

        public static VisibilityGrid<NetworkCell> Grid
        {
            get
            {
                if (grid == null)
                {
                    grid = new VisibilityGrid<NetworkCell>(50, 10);
                }
                return grid;
            }
        }
        private static VisibilityGrid<NetworkCell> grid;

        [SerializeField] private float updateIntervalSeconds = 2.2f;
        private float updateDelay;

        private void Awake()
        {
            updateDelay = UnityEngine.Random.Range(0f, updateIntervalSeconds);
        }

        private void Update()
        {
            if (!IsServer)
                return;

            updateDelay -= Time.deltaTime;

            if (updateDelay > 0f)
                return;

            Grid.Update();
            updateDelay = updateIntervalSeconds;
        }
    }
}