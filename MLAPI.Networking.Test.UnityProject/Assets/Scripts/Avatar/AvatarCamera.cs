using UnityEngine;

namespace Twindrums.TheWagaduChronicles.Client.Avatar
{
    public class AvatarCamera : MonoBehaviour
    {
        public Transform container;
        public Transform target;

        public bool zoomEnabled = true;

        public bool rotationEnabled = false;

        public int mouseButton = 1; // right button by default

        public float distance = 20;
        public float minDistance = 3;
        public float maxDistance = 20;

        public float zoomSpeedMouse = 1;
        public float zoomSpeedTouch = 0.2f;
        public float rotationSpeed = 2;

        public float xMinAngle = -40;
        public float xMaxAngle = 80;

        // the target position can be adjusted by an offset in order to foucs on a
        // target's head for example
        public Vector3 offset = Vector3.zero;

        // store rotation so that unity never modifies it, otherwise unity will put
        // it back to 360 as soon as it's <0, which makes a negative min angle
        // impossible
        Vector3 rotation;
        bool rotationInitialized;

        void LateUpdate()
        {
            if (!target) return;

            Vector3 targetPos = target.position + offset;

            // rotation and zoom should only happen if not in a UI right now
            //if (!Utils.IsCursorOverUserInterface())
            if (zoomEnabled || rotationEnabled)
            {
                // right mouse rotation if we have a mouse
                if (rotationEnabled
                    && Input.mousePresent)
                {
                    if (Input.GetMouseButton(mouseButton))
                    {
                        // initialize the base rotation if not initialized yet.
                        // (only after first mouse click and not in Awake because
                        //  we might rotate the camera inbetween, e.g. during
                        //  character selection. this would cause a sudden jump to
                        //  the original rotation from Awake otherwise.)
                        if (!rotationInitialized)
                        {
                            rotation = container.transform.eulerAngles;
                            rotationInitialized = true;
                        }

                        // note: mouse x is for y rotation and vice versa
                        rotation.y += Input.GetAxis("Mouse X") * rotationSpeed;
                        rotation.x -= Input.GetAxis("Mouse Y") * rotationSpeed;
                        rotation.x = Mathf.Clamp(rotation.x, xMinAngle, xMaxAngle);
                        container.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
                    }
                }
                else
                {
                    // forced 45 degree if there is no mouse to rotate (for mobile)
                    container.transform.rotation = Quaternion.Euler(new Vector3(45, 0, 0));
                }

                if (zoomEnabled)
                {
                    // zoom
                    float speed = Input.mousePresent ? zoomSpeedMouse : zoomSpeedTouch;
                    float step = Utils.GetZoomUniversal() * speed;
                    distance = Mathf.Clamp(distance - step, minDistance, maxDistance);
                }
            }

            // target follow
            container.transform.position = targetPos - (transform.rotation * Vector3.forward * distance);
        }
    }
}
