using System;
using _Script.Agent.Modules;
using UnityEngine;

namespace _Scripts.Agent
{
    public class OperatorSensor : MonoBehaviour, IModule, ITargetCaster
    {
        [SerializeField] private LayerMask targetLayer;
        private Agent _agent;

        public Collider[] SucceedColliders { get; } = new Collider[32]; //영원히 안바뀜. get만 하자 야르
        public int HitCount { get; private set; }

        public void Initialize(ModuleAgent moduleAgent)
        {
            _agent = moduleAgent as Agent;
        }
        
        /// <summary>
        /// 원형 TargetCaster.
        /// for문 돌릴 때 hitCount를 이용해서 범위 탐색을 하면 된다.
        /// </summary>
        /// <param name="radius">소환 될 원형의 크기</param>
        /// <param name="targetLayer">타게팅 대상</param>
        /// <param name="hitCount">적중한 적의 수. 이걸로 for문 돌리셈.</param>
        /// <returns></returns>
        public bool SearchTargetSphere(float radius, bool isResetOriginColliders = false)
        {
            if (isResetOriginColliders)
            {
                Array.Clear(SucceedColliders, 0, SucceedColliders.Length);
                //Array 전부 싹 클리어
                //Clear(대상, 첫번째 idx, 끝 idx);
            }
            //미리 캐싱해둔걸 쥐어주고 할당시켜주는 NonAlloc OverlapSphere. 모르면 멍충이
            //hitCount : Return값으로 찾기에 성공한 갯수를 반환해준다.
            HitCount = Physics.OverlapSphereNonAlloc(transform.position, radius, SucceedColliders, targetLayer);
            //아무것도 없으니까 false
            if (HitCount == 0)
            {
                return false;
            }

            return true;
        }
        public bool SearchTargetSphere(Vector3 pos, float radius, bool isResetOriginColliders = false)
        {
            if (isResetOriginColliders)
            {
                Array.Clear(SucceedColliders, 0, SucceedColliders.Length);
                //Array 전부 싹 클리어
                //Clear(대상, 첫번째 idx, 끝 idx);
            }
            //미리 캐싱해둔걸 쥐어주고 할당시켜주는 NonAlloc OverlapSphere. 모르면 멍충이
            //hitCount : Return값으로 찾기에 성공한 갯수를 반환해준다.
            HitCount = Physics.OverlapSphereNonAlloc(pos, radius, SucceedColliders, targetLayer);
            //아무것도 없으니까 false
            if (HitCount == 0)
            {
                return false;
            }

            return true;
        }
        public bool SearchTargetBox(Vector3 centerOffset, Vector3 size)
        {
            Vector3 worldCenter = transform.position + centerOffset;
            Vector3 extents = size * 0.5f;

            HitCount = Physics.OverlapBoxNonAlloc(worldCenter, extents, SucceedColliders, Quaternion.identity, targetLayer);
            return HitCount > 0;
        }
    }
}
