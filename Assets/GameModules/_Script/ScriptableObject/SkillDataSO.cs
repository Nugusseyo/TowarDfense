using System.Collections.Generic;
using _Script.Agent.CombatSystem;
using _Script.Agent.Modules.BattleSystem;
using UnityEngine;

namespace _Script.ScriptableObject
{
    public abstract class SkillDataSO : IndexSO
    {
        public string skillName;
        
        public DamageType damageType;
        public Vector2 knockbackPower;
        public List<Vector3Int> attackRange;
        public ConditionData[] Condition;
        
        public GameObject hitParticleEffect;
        public GameObject skillParticleEffect;
    }
}