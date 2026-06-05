using _Script.ScriptableObject.Event;
using UnityEngine;

namespace _Scripts.UI
{
    public static class InGameEvents
    {
        public static readonly GameEndEvent GameEndEvent = new GameEndEvent();
        public static readonly GameSwapEvent GameSwapEvent = new GameSwapEvent();
    }

    public class GameEndEvent : GameEvent
    {
        public bool IsLost { get; private set; }

        public GameEndEvent Init(bool isLost)
        {
            IsLost = isLost;
            return this;
        }
    }

    public class GameSwapEvent : GameEvent
    {
        public float NormalFade { get; private set; }

        public GameSwapEvent Init(float normalFade)
        {
            NormalFade = Mathf.Clamp01(normalFade);
            return this;
        }
    }
}