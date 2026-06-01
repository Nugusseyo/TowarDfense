using System.Collections.Generic;
using UnityEngine;

namespace GameModules._Script.Agent.CombatSystem.SortingSystem
{
    public abstract class SortingTargetSO : ScriptableObject
    {
        public abstract void SortingTarget(Collider[] colliders, int hitCount, Vector3 attackerPos, 
            int maxTargetCount, List<_Scripts.Agent.Agent> outTargets);
    }
}
