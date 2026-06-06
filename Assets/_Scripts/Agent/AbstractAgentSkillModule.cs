using System;
using _Script.Agent.Modules;
using _Scripts.Agent.Combat.Skills;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Agent
{
    public interface IAgentSkillModule
    {
        AbstractSkillDataSO SkillData { get; }
        float GetCooldownNormal { get; }
        bool CanUseSkill();
        void UseSkill();
    }

    public abstract class AgentSkillModule : MonoBehaviour, IModule, IAgentSkillModule
    {
        public UnityEvent OnSkill;

        [field: SerializeField] public AbstractSkillDataSO SkillData { get; private set; } 
        [field: SerializeField] public SkillConditionSO SkillCondition { get; private set; } 
        
        protected Agent _agent;
        
        private float _cooldown;
        private float _timer;
        public float GetCooldownNormal => Mathf.Clamp01(_timer / _cooldown);
        
        protected ITargetCaster _targetCaster;
        public void Initialize(ModuleAgent moduleAgent)
        {
            _agent = moduleAgent as Agent;
            Debug.Assert(_agent != null, $"Agent Skill Module인데 Agent가 아닙니다! Target : {gameObject.name}");

            _targetCaster = moduleAgent.GetModule<ITargetCaster>();
            Debug.Assert(_targetCaster != null, $"Target Caster가 누락되었습니다. Target : {gameObject.name}");
            
            _cooldown = _agent.AgentStatusSO.SkillAttackCooldown;
            
            if (SkillData != null)
            {
                SkillData = Instantiate(SkillData);
            }
        }

        private void Update()
        {
            if (_cooldown <= _timer || _agent.HealthModule.IsDead) return;
            
            _timer += Time.deltaTime;
        }

        public bool CanUseSkill()
        {
            if (_timer < _cooldown || _agent.HealthModule.IsDead)
                return false;

            return SkillCondition.TryUseSkill(_agent, _targetCaster, SkillData);
        }

        public virtual void UseSkill()
        {
            OnSkill?.Invoke();
            _timer = 0;
        }
        
        protected virtual void OnDestroy()
        {
            if (SkillData != null)
            {
                Destroy(SkillData);
            }
        }

    }
}
