using System;
using _Script.Agent.Modules;
using _Scripts.Agent;
using _Scripts.Agent.Player;
using UnityEngine;
using UnityEngine.Events;

namespace GameModules._Script.Agent.Operator
{
    public class OperatorHealerAttackModule : AbstractAgentAttackModule
    {
        
        private PlayerStateChange _playerStateChange;
        private AbstractOperator _operator;

        public override void Initialize(ModuleAgent moduleAgent)
        {
            base.Initialize(moduleAgent);
            
            _operator = agent as AbstractOperator;
        }

        private void Start()
        {
            _playerStateChange = _operator.PlayerStateChange;
            Debug.Assert(_playerStateChange != null, $"Operator에 StateChange Event가 존재하지 않습니다! Target : {gameObject.name}");
        }

        public override void AttackTarget()
        {
            base.AttackTarget();
            
            int actualHitCount = Mathf.Min(_attackTargets.Count, _attackCount);
            for (int i = 0; i < actualHitCount; ++i)
            {
                // AI : 여기서 실제 데미지를 주는 로직(예: _attackTargets[i].TakeDamage(...))을 실행하시면 됩니다.

                if (_attackTargets[i].TryGetComponent(out IHealable healable))
                {
                    healable.TakeHeal(_operator.AgentStatusSO.Damage);
                    OnAttack?.Invoke(_attackTargets[i].transform);
                }
            }
        }

        public override bool TryTargeting()
            => targetCaster.SearchTargetSphere(agent.AgentStatusSO.DetectRadius);

        public override void UseSkill()
        {
            if (_playerStateChange == null)
                _playerStateChange = _operator.PlayerStateChange;
            
            _playerStateChange.SendEventMessage(OperatorStateEnum.SKILL);
            Debug.Log("Use Skill!!!");
        }


        private void OnDrawGizmosSelected()
        {
            if (agent == null) return;
            if (agent.AgentStatusSO == null) return;
            
            Gizmos.color = Color.aquamarine;
            Gizmos.DrawWireSphere(transform.position, agent.AgentStatusSO.DetectRadius);
        }
    }
}