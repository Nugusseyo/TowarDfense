using System;
using _Script.Agent.FSM;
using _Script.Agent.Modules;
using _Script.Agent.Modules.BattleSystem;
using _Script.Agent.Modules.StatSystem;
using _Script.ScriptableObject;
using _Script.ScriptableObject.Event;
using Agents.FSM;
using GameLib.SoundSystem;
using UnityEngine;
using UnityEngine.Events;
using HealthModule = _Scripts.Agent;

namespace _Script.Agent
{
    public abstract class Agent : ModuleAgent, IDamageable //Enemy와 User가 공통적으로 가지고 있는 요소들을 Agent로 묶어서 정의.
    {
        //Health System
        //Attack System (Skill)
        
        public UnityEvent OnHit;
        public UnityEvent OnDeath;

        public Collider Collider { get; protected set; }
        protected AgentStateMachine _stateMachine;
        
        [SerializeField] protected StateListSO stateList;
        protected ISkillModule _skillModule;
        protected IStatModule _statModule;
        [field: SerializeField] protected EventChannelSO SoundEventChannel;
        [field: SerializeField] public EventChannelSO ExpUpEventChannel;
        [SerializeField] protected SoundClipSO getDamageSound;

        public bool IsDead { get; protected set; }
        protected Modules.HealthModule Health { get; private set; }

        protected override void Awake()
        {
            Collider = GetComponent<Collider>();
            base.Awake();   
        }

        protected override void Initialize() //이미 부모에서 GetModule을 할 조건이 갖추어져 있기 때문에 괜찮음.
        {
            base.Initialize();
            
            
            Health = GetModule<Modules.HealthModule>();
            
            Debug.Assert(Health != null, $"Agent {gameObject.name}가 HealthModule이 존재하지 않습니다!");
            
            _skillModule = GetModule<ISkillModule>();
            Debug.Assert(_skillModule != null, $"{gameObject.name}의 SkillModule이 존재하지 않습니다!");
            
            _statModule = GetModule<IStatModule>();
            Debug.Assert(_statModule != null, $"{gameObject.name}의 StatModule이 존재하지 않습니다.");
        }

        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            
            if(_stateMachine == null)
                _stateMachine = new AgentStateMachine(this, stateList.states);
            Health.OnHealthChanged += HandleHealthChaged;
        }

        

        private void OnDestroy()
        {
            if(Health != null)
                Health.OnHealthChanged -= HandleHealthChaged;
        }

        protected abstract void HandleHealthChaged(float prevHealth, float currentHealth, float max); //Operator쪽에서 처리해줄거임.
        //각 오퍼레이터마다 가지고 있는 특징이나 패시브가 여기 안에 포함됨.

        public virtual void GetDamage(DamageData damageData)
        {
            SoundEventChannel.RaiseEvent(SoundSystemEvents.PlaySoundEvent.Init(transform.position, getDamageSound));
        }
        
        private void Update()
        {
            if (_stateMachine == null)
            {
                _stateMachine = new AgentStateMachine(this, stateList.states);
            }
            _stateMachine.UpdateStateMachine();
        }
        public AgentState GetCurrentState() => _stateMachine.CurrentState;
        public void ChangePlayerState(PlayerStateEnum nextState) => _stateMachine.ChangeState((int)nextState);
        public void ChangeEnemyState(EnemyStateEnum nextState) => _stateMachine.ChangeState((int)nextState);
    }
}
