using _Script.Agent.Modules;
using _Scripts.Agent.Player;
using UnityEngine;

namespace GameModules._Script.Agent.Operator.AttackModule
{
    public class OperatorCheerLeaderAttackModule : AbstractAgentAttackModule
    {
        private PlayerStateChange _playerStateChange;
        private AbstractOperator _operator;

        public override void Initialize(ModuleAgent moduleAgent)
        {
            base.Initialize(moduleAgent);
            
            _operator = agent as AbstractOperator;
        }
        public override bool TryTargeting()
        {
            return false;
        }

        public override void UseSkill()
        {
            if (_playerStateChange == null)
                _playerStateChange = _operator.PlayerStateChange;
            
            _playerStateChange.SendEventMessage(OperatorStateEnum.SKILL);
            Debug.Log("Use Skill!!!");
        }
    }
}
