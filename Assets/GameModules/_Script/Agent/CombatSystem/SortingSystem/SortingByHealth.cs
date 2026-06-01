using System.Collections.Generic;
using _Script.Agent.Modules;
using UnityEngine;
using HealthModule = _Scripts.Agent.Combat.HealthModule;

namespace GameModules._Script.Agent.CombatSystem.SortingSystem
{
    [CreateAssetMenu(fileName = "Sorting By Health", menuName = "Sorting Rules/Sorting By Health")]
    public class SortingByHealth : SortingTargetSO
    {
        public override void SortingTarget(Collider[] colliders, int hitCount, Vector3 attackerPos, int maxTargetCount, List<_Scripts.Agent.Agent> outTargets)
        {
            List<(_Scripts.Agent.Agent agent, float hp, float distance)> targets = new List<(_Scripts.Agent.Agent, float, float)>();
            
            for (int i = 0; i < hitCount; i++)
            {
                Collider currentCollider = colliders[i];
                if (currentCollider == null) continue;

                if (currentCollider.TryGetComponent<_Scripts.Agent.Agent>(out _Scripts.Agent.Agent targetAgent))
                {
                    HealthModule healthModule = targetAgent.HealthModule;
                    
                    if (healthModule != null && healthModule.Health > 0)
                    {
                        float targetHp = healthModule.Health; 
                        float distance = (targetAgent.transform.position - attackerPos).sqrMagnitude;
                        targets.Add((targetAgent, targetHp, distance));
                    }
                    else
                        Debug.LogWarning("대상의 HealthModule이 존재하지 않네요...");
                }
            } //이거 코드 중복되는데 어떻게 못할까;
            
            targets.Sort((a, b) => 
            {
                int hpCompare = a.hp.CompareTo(b.hp);
                if (hpCompare != 0) return hpCompare; //차이가 0이 아니라면 (똑같지 않으면) 거리 비교 해야댐.
                
                return a.distance.CompareTo(b.distance);
            });
            
            int targetCountToTake = Mathf.Min(targets.Count, maxTargetCount);
            for (int i = 0; i < targetCountToTake; i++)
            {
                outTargets.Add(targets[i].agent);
            }
        }
    }
}
