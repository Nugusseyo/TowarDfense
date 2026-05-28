using System.Collections.Generic;

namespace GameModules._Script.Agent
{
    public interface IAgentAttackModule
    {
        List<_Scripts.Agent.Agent> AttackTargetList { get;}
        void AttackTarget();
        void SortingTargets();
        bool TryTargeting();
    }
}