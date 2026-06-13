using _Script.ScriptableObject.Event;

namespace _Scripts.UI
{
    public static class UIEvents
    {
        public static readonly ButtonUI ButtonUI = new ButtonUI();
        public static readonly AlarmUI AlarmUI = new AlarmUI();
    }

    public class ButtonUI : GameEvent
    {
        public ObjectButton Button { get; private set; }

        public ButtonUI Init(ObjectButton button)
        {
            Button = button;
            return this;
        }
    }

    public class AlarmUI : GameEvent
    {
        public string AlarmText { get; private set; }

        public AlarmUI Init(string alarmText)
        {
            AlarmText = alarmText;
            return this;
        }
    }
}