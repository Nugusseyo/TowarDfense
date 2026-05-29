using UnityEngine;

namespace _Scripts.Agent.Combat.Skills
{
    public abstract class AbstractSkillDataSO : ScriptableObject
    {
        [field: SerializeField] public float SkillRadius { get; private set; } = 10f;
        [SerializeField] private string description;
        public abstract void UseSkill(Agent agent, ITargetCaster caster);
    }
}
