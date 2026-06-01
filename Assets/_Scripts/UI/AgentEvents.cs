using _Script.ScriptableObject.Event;

namespace _Scripts.UI
{
    public static class AgentEvents
    {
        public static AgentOnUI AgentOnUI = new AgentOnUI();
        public static AgentInfoUI AgentInfoUI = new AgentInfoUI();
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

    public class AgentInfoUI : GameEvent
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