using System;
using _Script.ScriptableObject.Event;
using UnityEngine;

namespace _Script.Camera
{
    public class CameraSync : MonoBehaviour
    {
        [field: SerializeField] public EventChannelSO CameraAngleEventChannel { get; private set; }

        private void Awake()
        {
            
        }
    }
}
