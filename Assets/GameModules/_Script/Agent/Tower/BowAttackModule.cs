using _Script.Agent.Modules;
using _Scripts.Agent;
using UnityEngine;

namespace GameModules._Script.Agent.Tower
{
    public class BowAttackModule : AbstractAgentAttackModule
    {
        private TowerStateChange _towerStateChange;
        private _Scripts.Agent.Tower.Tower _tower;

        public override void Initialize(ModuleAgent moduleAgent)
        {
            base.Initialize(moduleAgent);
            
            _tower = agent as _Scripts.Agent.Tower.Tower;
        }

        private void Start()
        {
            _towerStateChange = _tower.TowerStateChange;
            Debug.Assert(_towerStateChange != null, $"Tower에 StateChange Event가 존재하지 않습니다! Target : {gameObject.name}");
        }

        public override void AttackTarget()
        {
            base.AttackTarget();
            
            int actualHitCount = Mathf.Min(_attackTargets.Count, _attackCount);
            for (int i = 0; i < actualHitCount; ++i)
            {
                if (_attackTargets[i].TryGetComponent(out IHealable healable))
                {
                    healable.TakeDamage(_tower.AgentStatusSO.Damage);
                    OnAttack?.Invoke(_attackTargets[i].transform);
                }
            }
        }

        public override bool TryTargeting()
            => targetCaster.SearchTargetSphere(agent.AgentStatusSO.DetectRadius);

        public override void UseSkill()
        {
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
