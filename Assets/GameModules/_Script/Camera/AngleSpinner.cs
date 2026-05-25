using UnityEngine;

namespace _Script.Camera
{
    public class AngleSpinner : MonoBehaviour
    {
        private Transform _cameraTransform;
        private void Awake()
        {
            _cameraTransform = GameObject.Find("CameraBundle").transform;
        }

        private void LateUpdate()
        {
            transform.forward = _cameraTransform.forward;
            transform.up = _cameraTransform.up;
        }
    }
}