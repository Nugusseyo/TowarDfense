using System;
using _Script.ScriptableObject.Event;
using _Script.Tools.Utility;
using UnityEngine;

namespace _Script.Camera
{
    public class MainCameraInfo : MonoBehaviour
    {

        public float twistAngle;
        [field: SerializeField] public bool IsTwist { get; private set; }

        [ContextMenu("Camera Twist")]
        public void ActiveAngleTwist()
        {
            if (IsTwist) return;
            IsTwist = true;
            transform.eulerAngles = new Vector3(70, twistAngle, twistAngle);
        }
        
        [ContextMenu("Camera Reset")]
        public void ResetAngleTwist()
        {
            if(!IsTwist) return;
            IsTwist = false;
            transform.eulerAngles = new Vector3(70, 0, 0);
        }
    }
}
