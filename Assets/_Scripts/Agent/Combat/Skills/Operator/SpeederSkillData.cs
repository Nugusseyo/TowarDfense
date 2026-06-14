using System.Collections;
using System.Collections.Generic;
using _Scripts.Managers.CostManager;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.Agent.Combat.Skills.Operator
{
    [CreateAssetMenu(fileName = "new Speeder Skill data", menuName = "Operator/Skill Data/Speeder Skill data")]
    public class SpeederSkillData : AbstractSkillDataSO
    {
        [field: SerializeField] public float SpeedModifier = 1.5f;
        [field: SerializeField] public float Duration = 3f;

        public override void UseSkill(Agent agent, ITargetCaster caster)
        {
            caster.SearchTargetSphere(SkillRadius);
            
            List<NavMeshAgent> affectedAgents = new List<NavMeshAgent>();

            for (int i = 0; i < caster.HitCount; ++i)
            {
                if (caster.SucceedColliders[i].TryGetComponent(out NavMeshAgent nav))
                {
                    nav.speed += SpeedModifier;
                    affectedAgents.Add(nav);
                }
            }
            
            if (affectedAgents.Count > 0 && agent is MonoBehaviour monoAgent)
            {
                monoAgent.StartCoroutine(ResetSpeedRoutine(affectedAgents, SpeedModifier));
            }
        }
        
        private IEnumerator ResetSpeedRoutine(List<NavMeshAgent> agentsToReset, float modifierAmount)
        {
            yield return new WaitForSeconds(Duration);

            foreach (NavMeshAgent nav in agentsToReset)
            {
                if (nav != null)
                {
                    nav.speed -= modifierAmount;
                }
            }
        }
    }
}