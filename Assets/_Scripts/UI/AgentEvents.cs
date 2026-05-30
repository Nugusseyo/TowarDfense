using _Script.ScriptableObject.Event;

namespace _Scripts.UI
{
    public static class AgentEvents
    {
        public static AgentOnUI AgentOnUI = new AgentOnUI();
    }

    public class AgentOnUI : GameEvent
    {
        public Agent.Agent NextAgent { get; private set; }

        public AgentOnUI Init(Agent.Agent agent)
        {
            NextAgent = agent;
            return this;
        }
    }
}