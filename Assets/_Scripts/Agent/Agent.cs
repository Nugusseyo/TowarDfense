using System;
using _Script.Agent.Modules;
using _Script.Agent.Modules.StatSystem;
using _Scripts.Agent.Player;
using GameModules._Script.Agent;
using Unity.Behavior;
using UnityEngine;
using HealthModule = _Scripts.Agent.Combat.HealthModule;

namespace _Scripts.Agent
{
    public abstract class Agent : ModuleAgent, IHealable
    {
        public BehaviorGraphAgent AgentBT { get; private set; }
        public IAgentAttackModule AttackModule { get; private set; }
        public HealthModule HealthModule { get; private set; }

        [field:SerializeField] public OperatorStatusSO AgentStatusSO { get; protected set; }
        [field:SerializeField] public AgentUIDataSO UIData { get; private set; }
        [field:SerializeField] public TagSO AgentTag { get; private set; }
        protected override void Awake()
        {
            AgentStatusSO = Instantiate(AgentStatusSO);
            base.Awake();
            AgentBT = GetComponent<BehaviorGraphAgent>();
            Debug.Assert(AgentBT != null, $"BT가 없잖아요 멍충아ㅏㅏ 정신차려 조윤규. Target : {gameObject.name}");
            /*
            TargetCaster = GetModule<ITargetCaster>();
            Debug.Assert(TargetCaster != null, $"Agent는 무조건 TargetCaster가 존재해야합니다. Target : {gameObject.name}");
            
            Renderer = GetModule<IAgentRenderer>();
            Debug.Assert(Renderer != null, $"Agent는 무조건 IAgentRenderer 존재해야 합니다. Target : {gameObject.name}");
            Trigger = GetModule<IAnimationTrigger>();
            Debug.Assert(Trigger != null, $"Agent는 무조건 Trigger가 존재해야 합니다. Target : {gameObject.name}");
            */

            AttackModule = GetModule<IAgentAttackModule>();
            Debug.Assert(AttackModule != null, $"Agent가 AttackModule이 없습니다! Target : {gameObject.name}");
            HealthModule = GetModule<HealthModule>();
            Debug.Assert(HealthModule != null, $"Agent에는 무조건 HealthModule이 존재해야 합니다. Target : {gameObject.name}");
            HealthModule.OnDeath.AddListener(OnDeath);
        }

        protected virtual void OnDestroy()
        {
            if(HealthModule != null)
                HealthModule.OnDeath.RemoveListener(OnDeath);

            if (AgentStatusSO != null)
                Destroy(AgentStatusSO);
        }

        public void SetVariable<T>(string variableName, T value)
        {
            Debug.Assert(!string.IsNullOrEmpty(variableName), $"Variable Name이 공백입니다. Target : {gameObject.name}");

            if (AgentBT.GetVariable(variableName, out BlackboardVariable<T> getValue))
            {
                getValue.Value = value;
                return;
            }
            
            Debug.LogWarning($"Variable Name이 존재하지 않습니다. Target : {gameObject.name}, Name : {variableName}");
        }

        public bool GetVariable<T>(string variableName, out BlackboardVariable<T> value)
        {
            Debug.Assert(!string.IsNullOrEmpty(variableName), $"Variable Name이 공백입니다. Target : {gameObject.name}");

            return AgentBT.GetVariable<T>(variableName, out value);
        }

        public virtual void TakeHeal(int healAmount)
        {
            HealthModule.TakeHeal(healAmount);
        }

        public virtual void TakeDamage(int damageAmount)
        {
            HealthModule.TakeDamage(damageAmount);
        }

        public abstract void OnDeath();
        public virtual void UseSkill() => AttackModule.UseSkill();
    }
}
