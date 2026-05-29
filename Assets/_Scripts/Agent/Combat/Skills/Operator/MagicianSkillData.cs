using UnityEngine;

namespace _Scripts.Agent.Combat.Skills.Operator
{
    [CreateAssetMenu(fileName = "new Magician Skill data", menuName = "Operator/Skill Data/Magician Skill data")]
    public class MagicianSkillData : AbstractSkillDataSO
    {
        public int healValue;
        public override void UseSkill(Agent agent, ITargetCaster caster)
        {
            //공격 범위 내 모든 시민들 치료.
            for (int i = 0; i < caster.HitCount; ++i)
            {
                if (caster.SucceedColliders[i].TryGetComponent(out IHealable healable))
                {
                    healable.TakeHeal(healValue);
                }
            }
        }
    }
}
