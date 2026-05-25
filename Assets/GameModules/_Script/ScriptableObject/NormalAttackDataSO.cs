using System.Collections.Generic;
using UnityEngine;

namespace _Script.ScriptableObject
{
    [CreateAssetMenu(fileName = "new NormalAttackData SO", menuName = "Data/Combat/Normal Attack Data", order = 15)]
    public class NormalAttackDataSO : SkillDataSO
    {
        public List<Vector3Int> baseAttackRange; // 0~1 정예화 상태인 오퍼레이터의 Attack Range
    }
}