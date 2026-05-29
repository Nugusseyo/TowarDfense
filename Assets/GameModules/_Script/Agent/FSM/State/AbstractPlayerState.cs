using _Script.Agent.CombatSystem;
using _Script.Agent.FSM;
using _Script.Agent.Modules;
using _Script.Agent.Modules.BattleSystem;
using _Script.Agent.Modules.StatSystem;
using _Script.Agent.Operator;
using _Script.ScriptableObject;
using _Scripts.Agent;
using GameModules._Script.Agent.CombatSystem;
using UnityEngine;

namespace _Script.Agent.FSM.State
{
    public abstract class AbstractPlayerState : AgentState
    {
        protected PlayerOperator _playerOperator;
        protected AnimationHashSO _animationHash;
        protected IAnimationTrigger _trigger;
        protected IOperatorTargetCaster TargetCaster;
        protected ISkillModule _skillModule;

        protected DamageData _damageData = new DamageData();
        
        public AbstractPlayerState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
            _playerOperator = agent as PlayerOperator;
            Debug.Assert(_playerOperator != null, $"Operator의 \"PlayerOperator\"가 존재하지 않습니다.");
            _animationHash = hash;
            
            _trigger = agent.GetModule<IAnimationTrigger>();
            Debug.Assert(_trigger != null, $"Operator의 \"AnimationTrigger\"가 존재하지 않습니다!");
            
            TargetCaster = agent.GetModule<IOperatorTargetCaster>();
            Debug.Assert(TargetCaster != null, $"Operator의 \"DamageCaster\"가 존재하지 않습니다!");
            
            _skillModule = agent.GetModule<ISkillModule>();
            Debug.Assert(_skillModule != null, $"Operator의 \"SkillModule\"가 존재하지 않습니다!");
        }   
    }
}