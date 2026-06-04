using System.Collections;
using System.Numerics;
using _Scripts.Agent;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace GameModules._Script.Agent.Tower
{
    public class MortarAttackModule : AbstractAgentAttackModule
    {
        [SerializeField] private Transform attackTrm;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletRadius;
        [SerializeField] private int bulletTargetCount;
        public override void AttackTarget()
        {
            base.AttackTarget();

            if (_attackCount == 0) return;
            if (_attackTargets == null || _attackTargets.Count <= 0) return;

            Vector3 targetPos = _attackTargets[0].transform.position;
            StartCoroutine(ShootTarget(targetPos));
        }

        private IEnumerator ShootTarget(Vector3 targetPos)
        {
            GameObject bullet = Instantiate(bulletPrefab, attackTrm.position, Quaternion.identity);
            float duration = Vector3.Distance(transform.position, targetPos) / 10f;
            float curTime = 0;

            Vector3 attackPos = attackTrm.position;
            
            Debug.Log("Duration : " + duration);
            
            while (curTime <= duration)
            {
                float t =  Mathf.Clamp01(curTime / duration);
                Vector3 bulletPos = Vector3.Lerp(attackPos, targetPos, t);
                bulletPos.y += curve.Evaluate(t) * 3;
                bullet.transform.position = bulletPos;
                
                curTime += Time.deltaTime;
                yield return null;
            }

            //Particle Play 해줘야댐;;
            Destroy(bullet);
            
            if (!targetCaster.SearchTargetSphere(bullet.transform.position, bulletRadius) || targetCaster.HitCount <= 0) yield break;
            
            for (int i = 0; i < targetCaster.HitCount; i++)
            {
                if (targetCaster.SucceedColliders[i].TryGetComponent<IHealable>(out IHealable healable))
                {
                    healable.TakeDamage(agent.AgentStatusSO.Damage);
                }
            }
        }

        public override bool TryTargeting() 
            => targetCaster.SearchTargetSphere(agent.AgentStatusSO.DetectRadius);

        public override void UseSkill()
        {
            
        }
    }
}
