using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Cameras
{
    public class CameraSlider : MonoBehaviour
    {
        [Header("Transform")]
        [SerializeField] private Transform startTrm;
        [SerializeField] private Transform endTrm;
    
        [Header("Scroll")]
        [SerializeField] private ScrollRect scroll;

        private Camera _mainCam;

        public Camera MainCam
        {
            get
            {
                if(_mainCam == null)
                    _mainCam = Camera.main;
                return _mainCam;
            }
        }

        private void Update()
        {
            if (startTrm == null || endTrm == null || scroll == null || MainCam == null) return;
            
            _mainCam.transform.position = Vector3.Lerp(startTrm.position, endTrm.position, scroll.horizontalNormalizedPosition);
        }
    }
}
