using UnityEngine;

namespace _Scripts.Agent.Combat.Skills.Operator
{
    [CreateAssetMenu(fileName = "new Sniper Skill data", menuName = "Operator/Skill Data/Sniper Skill data")]
    public class SniperSkillData : AbstractSkillDataSO
    {
        [SerializeField] private int damage;

        private Agent _agent;
        
        private IAnimationTrigger _agentTrigger;
        private int _prevDamage;
        public override void UseSkill(Agent agent, ITargetCaster caster)
        {
            _agent = agent;
            _prevDamage = agent.AgentStatusSO.Damage;
            agent.AgentStatusSO.SetDamage(damage + _prevDamage);
            agent.AttackModule.AttackTarget();
            _agentTrigger = agent.GetModule<IAnimationTrigger>();
            _agentTrigger.OnAttackTrigger += HandleAttack;
        }

        private void HandleAttack()
        {
            _agent.AgentStatusSO.SetDamage(_prevDamage);
            _agentTrigger.OnAttackTrigger -= HandleAttack;
        }
    }
}
