using UnityEngine;
using System.Collections;
using MLAPI;
using System;
using Twindrums.TheWagaduChronicles.Visibility;

namespace Twindrums.TheWagaduChronicles.NetworkVisibility
{
    public class NetworkedVisibility : NetworkedBehaviour
    {
        public static VisibilityGrid<Cell> Grid
        {
            get
            {
                if (grid == null)
                {
                    grid = new VisibilityGrid<Cell>(50, 10);
                }
                return grid;
            }
        }
        private static VisibilityGrid<Cell> grid;

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