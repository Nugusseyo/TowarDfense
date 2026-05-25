using System;
using System.Collections.Generic;
using System.Linq;
using _Script.Agent.Modules.BattleSystem;
using UnityEngine;

namespace _Script.ScriptableObject
{
    [CreateAssetMenu(fileName = "new UltimateSkill Data", menuName = "Data/Combat/Ultimate Skill Data", order = 0)]
    public class UltimateSkillDataSO : SkillDataSO
    {
        public int spValue = 5;
        public GetSpType[] spType;

        public bool isBullet;
        
        public float duration = 10f;
        
        public StatSO[] effectTarget;

        private void OnValidate()
        {
            spType = spType.Distinct().ToArray();
        }
    }
}