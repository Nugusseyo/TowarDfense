using System.Collections.Generic;
using UnityEngine;

namespace GameModules._Script.Agent.CombatSystem.SortingSystem
{
    [CreateAssetMenu(fileName = "Sorting By Distance", menuName = "Sorting Rules/Sorting By Distance")]
    public class SortingByDistance : SortingTargetSO
    {
        public override void SortingTarget(Collider[] colliders, int hitCount, Vector3 attackerPos, int maxTargetCount, List<_Scripts.Agent.Agent> outTargets)
        {
            List<(_Scripts.Agent.Agent agent, float distance)> targets = new List<(_Scripts.Agent.Agent, float)>();

            for (int i = 0; i < hitCount; i++)
            {
                Collider currentCollider = colliders[i];
                if (currentCollider == null) continue;

                if (currentCollider.TryGetComponent<_Scripts.Agent.Agent>(out _Scripts.Agent.Agent targetAgent))
                {
                    float distance = (targetAgent.transform.position - attackerPos).sqrMagnitude;
                    targets.Add((targetAgent, distance));
                }
            }
            
            //먼 거리로 오름차순
            targets.Sort((a, b) => b.distance.CompareTo(a.distance));

            int targetCountToTake = Mathf.Min(targets.Count, maxTargetCount);
            for (int i = 0; i < targetCountToTake; i++)
            {
                outTargets.Add(targets[i].agent);
            }
        }
    }
}
