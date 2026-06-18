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
        void UseSkillStart();
        bool IsUsingSkill { get; set; }
    }

    public abstract class AgentSkillModule : MonoBehaviour, IModule, IAgentSkillModule
    {
        public UnityEvent OnSkill;
        public UnityEvent OnSkillStart;

        [field: SerializeField] public AbstractSkillDataSO SkillData { get; private set; } 
        [field: SerializeField] public SkillConditionSO SkillCondition { get; private set; } 
        
        protected Agent _agent;
        
        private float _cooldown;
        private float _timer;
        public float GetCooldownNormal => Mathf.Clamp01(_timer / _cooldown);

        public bool IsUsingSkill { get; set; } = false;

        protected ITargetCaster _targetCaster;
        protected IAnimationTrigger _animationTrigger;
        public void Initialize(ModuleAgent moduleAgent)
        {
            _agent = moduleAgent as Agent;
            Debug.Assert(_agent != null, $"Agent Skill Module인데 Agent가 아닙니다! Target : {gameObject.name}");

            _targetCaster = moduleAgent.GetModule<ITargetCaster>();
            Debug.Assert(_targetCaster != null, $"Target Caster가 누락되었습니다. Target : {gameObject.name}");
            
            _animationTrigger = moduleAgent.GetModule<IAnimationTrigger>();
            Debug.Assert(_targetCaster != null, $"AnimationTrigger가 누락되었습니다. Target : {gameObject.name}");

            _animationTrigger.OnSkillStartTrigger += HandleSkillStart;
            
            _cooldown = _agent.AgentStatusSO.SkillAttackCooldown;
            
            if (SkillData != null)
            {
                SkillData = Instantiate(SkillData);
            }
        }

        private void HandleSkillStart()
        {
            OnSkillStart?.Invoke();
        }


        private void Update()
        {
            if (_cooldown <= _timer || _agent.HealthModule.IsDead || IsUsingSkill) return;
            
            _timer += Time.deltaTime;
        }

        public bool CanUseSkill()
        {
            if (_timer < _cooldown || _agent.HealthModule.IsDead)
                return false;

            Debug.Log("CanUseSkill 허용됨");
            return SkillCondition.TryUseSkill(_agent, _targetCaster, SkillData);
        }

        public virtual void UseSkill()
        {
            OnSkill?.Invoke();
        }

        public void UseSkillStart()
        {
            _timer = 0;
            IsUsingSkill = true;
        }
        
        protected virtual void OnDestroy()
        {
            if (SkillData != null)
            {
                Destroy(SkillData);
            }
            if(_animationTrigger != null)
                _animationTrigger.OnSkillStartTrigger -= HandleSkillStart;
        }

    }
}
