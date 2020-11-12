using UnityEngine;
using System.Collections;

namespace Twindrums.TheWagaduChronicles.Client.Avatar
{
    public class AvatarCameraConnector : MonoBehaviour
    {
        private void Start()
        {
            var camera = Camera.main.GetComponent<AvatarCamera>();

            if (camera == null || camera.target != null)
                return;

            camera.target = this.transform;
        }
    }
}
