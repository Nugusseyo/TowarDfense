using _Scripts.Agent.Combat.Skills;
using UnityEngine;

namespace _Scripts.Agent.Skill
{
    [CreateAssetMenu(fileName = "new Always Available Skill Condition", menuName = "Agent/Conditions/Always Available")]
    public class AlwaysAvailableSkillCondition : SkillConditionSO
    {
        public override bool TryUseSkill(Agent agent, ITargetCaster caster, AbstractSkillDataSO skillData)
        {
            return true;
        }
    }
}
