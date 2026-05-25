namespace _Script.ScriptableObject.Event
{
    public static class OperatorEvents
    {
        public static readonly AddExpEvent AddExpEvent = new AddExpEvent();
        public static readonly LevelUpEvent LevelUpEvent = new LevelUpEvent();
    }

    public class LevelUpEvent : GameEvent
    {
        public int NewLevel { get; private set; }

        public LevelUpEvent Init(int newLevel)
        {
            NewLevel = newLevel;
            return this;
        }
    }

    public class AddExpEvent : GameEvent
    {
        public int Amount { get; private set; }

        public AddExpEvent Init(int amount)
        {
            Amount = amount;
            return this;
        }
    }
}