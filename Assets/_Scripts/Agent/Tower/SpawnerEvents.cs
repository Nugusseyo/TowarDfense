using _Script.ScriptableObject.Event;

namespace _Scripts.Agent.Tower
{
    public class SpawnerEvents
    {
        public static SpawnComplete SpawnComplete = new SpawnComplete();
    }

    public class SpawnComplete : GameEvent
    {
        public bool IsComplete = false;
        public SpawnComplete Init(bool isComplete)
        {
            IsComplete = isComplete;
            return this;
        }
    }
}