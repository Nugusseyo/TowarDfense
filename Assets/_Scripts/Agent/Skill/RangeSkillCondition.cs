using _Scripts.Agent.Combat.Skills;
using UnityEngine;

namespace _Scripts.Agent.Skill
{
    [CreateAssetMenu(fileName = "new Range Skill Condition", menuName = "Agent/Conditions/Range Condition")]
    public class RangeSkillCondition : SkillConditionSO
    {
        public override bool TryUseSkill(Agent agent, ITargetCaster caster, AbstractSkillDataSO skillData)
        {
            return caster.SearchTargetSphere(skillData.SkillRadius);
        }
    }
}
