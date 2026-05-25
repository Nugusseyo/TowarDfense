using System;
using _Script.ScriptableObject;
using UnityEngine;

namespace _Script.Agent.Modules.StatSystem
{
    [Serializable]
    public class AgentStatus
    {
        [field: SerializeField] public StatSO TargetStat { get; set; }
        [SerializeField] public bool isUsingNewValue = true;

        [SerializeField]
        public float newValue;

        public StatSO CreateStatSO()
        {
            StatSO stat = TargetStat.Clone() as StatSO;
            
            Debug.Assert(stat != null, $"Stat SO : {TargetStat.Name}이 Null입니다!");
            
            if(isUsingNewValue)
                stat.BaseValue = newValue;

            return stat;
        }
    }
}