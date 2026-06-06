using UnityEngine;

namespace _Scripts.Agent.Combat.Skills.Operator
{
    [CreateAssetMenu(fileName = "new OHealer Skill data", menuName = "Operator/Skill Data/OHealer Skill data")]
    public class OHealerSkillData : AbstractSkillDataSO
    {
        [field: SerializeField] public int DamageValue { get; private set; } = 200;
        private int _prevDamage;

        private IAnimationTrigger _trigger;
        private Agent _agent;
        private bool _isEffected = false;
        public override void UseSkill(Agent agent, ITargetCaster caster)
        {
            if (_isEffected) return;
            _agent = agent;
            _prevDamage = agent.AgentStatusSO.Damage;
            agent.AgentStatusSO.SetDamage(_prevDamage + DamageValue);
            _trigger = agent.GetModule<IAnimationTrigger>();
            if (_trigger == null)
            {
                agent.AgentStatusSO.SetDamage(_prevDamage); //트리거 없으면 그냥 다시 초기화
                return;
            }

            _isEffected = true;
            _trigger.OnAttackTrigger += HandleOperatorAttack;

        }

        private void HandleOperatorAttack()
        {
            Debug.Log("적용 완료" + DamageValue + _prevDamage);
            
            if(_agent != null)
                _agent.AgentStatusSO.SetDamage(_prevDamage);
            
            if(_trigger != null)
                _trigger.OnAttackTrigger -= HandleOperatorAttack;
            
            _isEffected = false;
        }
    }
}
