using _Scripts.Managers.CostManager;
using UnityEngine;

namespace _Scripts.Agent.Combat.Skills.Operator
{
    [CreateAssetMenu(fileName = "new Tanker Skill data", menuName = "Operator/Skill Data/Tanker Skill data")]
    public class TankerSkillData : AbstractSkillDataSO
    {
        public override void UseSkill(Agent agent, ITargetCaster caster)
        {
            CostManager.Instance.Cost += (int)SkillRadius;
        }
    }
}
