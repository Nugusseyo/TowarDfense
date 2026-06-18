using System;
using _Script.Agent.Modules;
using UnityEngine;

namespace _Scripts.Agent
{
    public class AgentTrigger : MonoBehaviour, IModule, IAnimationTrigger
    {
        private ModuleAgent _moduleAgent;
        public void Initialize(ModuleAgent moduleAgent)
        {
            _moduleAgent = moduleAgent;
        }
        
        public event Action OnAnimationEnd;
        public event Action OnAttackTrigger;
        public event Action OnSkillStartTrigger;
        
        public void ResetEndTrigger()
        {
            OnAnimationEnd = null;
        }

        public void ResetAttackTrigger()
        {
            OnAttackTrigger = null;
        }

        public void OnAnimationEndEvent() => OnAnimationEnd?.Invoke();
        public void OnAttackTriggerEvent() => OnAttackTrigger?.Invoke();
        public void OnSkillTriggerEvent() => OnSkillStartTrigger?.Invoke();
    }
}
