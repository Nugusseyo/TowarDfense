using _Script.ScriptableObject.Event;

namespace _Scripts.UI
{
    public static class UIEvents
    {
        public static readonly ButtonUI ButtonUI = new ButtonUI();
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
}