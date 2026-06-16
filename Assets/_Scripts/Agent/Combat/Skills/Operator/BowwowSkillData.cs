using UnityEngine;

namespace _Scripts.Agent.Combat.Skills.Operator
{
    [CreateAssetMenu(fileName = "new Bowwow Skill data", menuName = "Operator/Skill Data/Bowwow Skill data")]
    public class BowwowSkillData : AbstractSkillDataSO
    {
        
        public override void UseSkill(Agent agent, ITargetCaster caster)
        {
            Animator animator = agent.GetModule<AgentRenderer>().Animator;
            if(animator.speed <= 4f)
                animator.speed += 0.1f;

            agent.AgentStatusSO.SetDamage(agent.AgentStatusSO.Damage + 1);
        }
    }
}
