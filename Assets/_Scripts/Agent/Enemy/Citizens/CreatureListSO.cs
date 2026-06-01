using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Agent.Enemy.Citizens
{
    [CreateAssetMenu(fileName = "new CreatureList SO", menuName = "Creature/Creature List", order = 0)]
    public class CreatureListSO : ScriptableObject
    {
        public List<CreatureInfo> CreatureInfos = new List<CreatureInfo>();
    }
}