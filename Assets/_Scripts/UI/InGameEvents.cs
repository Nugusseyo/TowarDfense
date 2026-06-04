using _Script.ScriptableObject.Event;

namespace _Scripts.UI
{
    public static class InGameEvents
    {
        public static GameEndEvent GameEndEvent = new GameEndEvent();
    }

    public class GameEndEvent : _Script.ScriptableObject.Event.GameEvent
    {
        public bool IsLost { get; private set; }

        public GameEndEvent Init(bool isLost)
        {
            IsLost = isLost;
            return this;
        }
    }
}