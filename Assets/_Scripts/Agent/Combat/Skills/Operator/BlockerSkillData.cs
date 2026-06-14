using UnityEngine;

namespace _Scripts.Agent.Combat.Skills.Operator
{
    [CreateAssetMenu(fileName = "new Blocker Skill data", menuName = "Operator/Skill Data/Blocker Skill data")]
    public class BlockerSkillData : AbstractSkillDataSO
    {
        public override void UseSkill(Agent agent, ITargetCaster caster)
        {
            //공격범위 내 타워 타게팅
            caster.SearchTargetSphere(SkillRadius);
            for (int i = 0; i < caster.HitCount; ++i)
            {
                if (caster.SucceedColliders[i].TryGetComponent(out Tower.Tower tower))
                {
                    tower.ShutDownTower();
                }
            }
        }
    }
}
