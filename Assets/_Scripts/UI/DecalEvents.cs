using _Script.ScriptableObject.Event;

namespace _Scripts.UI
{
    public static class DecalEvents
    {
        public static DecalShow DecalShow = new DecalShow();
    }

    public class DecalShow : GameEvent
    {
        public bool Show;
        public DecalShow Init(bool show)
        {
            Show = show;
            return this;
        }
    }
}