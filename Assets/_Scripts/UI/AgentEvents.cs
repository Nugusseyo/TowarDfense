using _Script.ScriptableObject.Event;

namespace _Scripts.UI
{
    public static class AgentEvents
    {
        public static readonly AgentOnUI AgentOnUI = new AgentOnUI();
        public static readonly AgentInfoUI AgentInfoUI = new AgentInfoUI();
    }

    public class AgentOnUI : _Script.ScriptableObject.Event.GameEvent
    {
        public Agent.Agent NextAgent { get; private set; }

        public AgentOnUI Init(Agent.Agent agent)
        {
            NextAgent = agent;
            return this;
        }
    }

    public class AgentInfoUI : _Script.ScriptableObject.Event.GameEvent
    {
        public Agent.Agent Agent { get; private set; }
        public bool IsActive { get; private set; }

        public AgentInfoUI Init(Agent.Agent agent, bool isActive)
        {
            Agent = agent;
            IsActive = isActive;
            return this;
        }
    }
}