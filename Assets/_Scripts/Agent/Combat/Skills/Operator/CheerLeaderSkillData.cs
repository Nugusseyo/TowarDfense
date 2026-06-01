using _Scripts.Managers.CostManager;
using UnityEngine;

namespace _Scripts.Agent.Combat.Skills.Operator
{
    [CreateAssetMenu(fileName = "new CheerLeader Skill data", menuName = "Operator/Skill Data/CheerLeader Skill data")]
    public class CheerLeaderSkillData : AbstractSkillDataSO
    {
        public override void UseSkill(Agent agent, ITargetCaster caster)
        {
            CostManager.Instance.Cost += (int)SkillRadius;
        }
    }
}
