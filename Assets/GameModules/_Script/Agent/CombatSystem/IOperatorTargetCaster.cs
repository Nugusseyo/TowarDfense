using System.Collections.Generic;
using UnityEngine;

namespace GameModules._Script.Agent.CombatSystem
{
    public interface IOperatorTargetCaster
    {
        bool CastEnemy(Collider owner);
        List<Collider> GetAttackTarget();
    }
}