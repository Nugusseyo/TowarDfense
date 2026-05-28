using UnityEngine;

namespace _Scripts.Agent
{
    public interface ITargetCaster
    {
        Collider[] SucceedColliders { get; } //영원히 안바뀜. get만 하자 야르
        int HitCount { get; }

        /// <summary>
        /// 원형 TargetCaster.
        /// for문 돌릴 때 hitCount를 이용해서 범위 탐색을 하면 된다.
        /// </summary>
        /// <param name="radius">소환 될 원형의 크기</param>
        /// <param name="targetLayer">타게팅 대상</param>
        /// <param name="hitCount">적중한 적의 수. 이걸로 for문 돌리셈.</param>
        /// <returns></returns>
        bool SearchTargetSphere(float radius, bool isResetOriginColliders = false);

        bool SearchTargetBox(Vector3 centerOffset, Vector3 size);
    }
}