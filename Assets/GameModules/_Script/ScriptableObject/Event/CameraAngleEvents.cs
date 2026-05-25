using UnityEngine;

namespace _Script.ScriptableObject.Event
{
    public static class CameraAngleEvents
    {
        public static readonly CameraAngleEvent CameraAngleEvent = new CameraAngleEvent();
    }

    public class CameraAngleEvent : GameEvent
    {
        public float CameraAngle { get; private set; }

        public CameraAngleEvent Init(float cameraAngle)
        {
            CameraAngle = cameraAngle;
            return this;
        }
    }
}