using _Scripts.Agent.Combat.Skills;
using UnityEngine;

namespace _Scripts.Agent
{
    public abstract class SkillConditionSO : ScriptableObject
    {
        public abstract bool TryUseSkill(Agent agent, ITargetCaster caster, AbstractSkillDataSO skillData);
    }
}