using System;
using Unity.Behavior;
using UnityEngine;

namespace _Scripts.Enemy
{
    public class AbstractEnemy : MonoBehaviour
    {
        protected const string EnemyString = "Enemy";
        public BehaviorGraphAgent EnemyBT { get; private set; }

        private void Awake()
        {
            EnemyBT = GetComponent<BehaviorGraphAgent>();
            Debug.Assert(EnemyBT != null, $"Enemy는 무조건 BehaviourGraphAgent가 있어야 합니다;;");
        }

        private void Start()
        {
            EnemyBT.SetVariableValue(EnemyString, this);
        }
    }
}
