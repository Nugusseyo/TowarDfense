using System;
using UnityEngine;

namespace _Script.Agent.Enemy
{
    [Serializable]
    public class EnemyRoute
    {
        public Transform moveTransform;
        public Vector3 offset;
        public float waitSecond;
    }
}