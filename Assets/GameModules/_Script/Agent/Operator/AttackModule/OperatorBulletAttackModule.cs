using System;
using System.Collections;
using _Scripts.Agent;
using _Scripts.Agent.Player;
using UnityEngine;
using UnityEngine.Events;

namespace GameModules._Script.Agent.Operator.AttackModule
{
    public class OperatorBulletAttackModule : AbstractAgentAttackModule
    {
        public UnityEvent<Vector3> OnBulletExplosion;
        [SerializeField] private Transform attackTrm;
        [SerializeField] private GameObject bulletPrefab; 
        [SerializeField] private float bulletSpeed = 15f;
        [SerializeField] private float destroyTime = 3f;

        [SerializeField] private bool isUsingSkill;

        [Header("Skill Setting")] 
        [SerializeField] private int targetCount;
        
        private int _counter = 0;
        private IAgentSkillModule _skillModule;
        private PlayerStateChange _playerStateChange;
        private AbstractOperator _operator;

        private void Start()
        {
            _operator = agent as AbstractOperator;
        }

        public override void AttackTarget()
        {
            base.AttackTarget();

            if (_attackTargets.Count <= 0) return;
            _Scripts.Agent.Agent targetAgent = _attackTargets[0];
            if (targetAgent == null) return;
            
            StartCoroutine(ShootLinearTarget(targetAgent));
        }

        private IEnumerator ShootLinearTarget(_Scripts.Agent.Agent targetAgent)
        {
            Vector3 startPos = attackTrm != null ? attackTrm.position : transform.position;
            GameObject bullet = Instantiate(bulletPrefab, startPos, Quaternion.identity);
            
            int damage = base.agent.AgentStatusSO.Damage;
            Vector3 targetPos = targetAgent.transform.position;
            float distance = Vector3.Distance(startPos, targetPos);
            float duration = distance / bulletSpeed;
            float curTime = 0;

            if (duration <= 0) duration = 0.1f;

            bullet.transform.LookAt(targetPos);

            while (curTime <= duration)
            {
                if (targetAgent == null || !targetAgent.gameObject.activeInHierarchy)
                {
                    DestroyImmediate(bullet);
                    break;
                }
                
                targetPos = targetAgent.transform.position;

                float t = Mathf.Clamp01(curTime / duration);
                
                Vector3 bulletPos = Vector3.Lerp(startPos, targetPos, t);
                bullet.transform.position = bulletPos;
                bullet.transform.LookAt(targetPos);

                curTime += Time.deltaTime;
                yield return null;
            }
            
            if (bullet != null)
            {
                if (bullet.TryGetComponent(out MeshRenderer mainMesh))
                {
                    mainMesh.enabled = false;
                }
                else if (bullet.GetComponentInChildren<MeshRenderer>() != null)
                {
                    bullet.GetComponentInChildren<MeshRenderer>().enabled = false;
                }

                OnBulletExplosion?.Invoke(bullet.transform.position);
                Destroy(bullet, destroyTime);
            }
            
            if (targetAgent != null && targetAgent.gameObject.activeInHierarchy)
            {
                targetAgent.TakeDamage(damage);
                _counter++;
            }

            if (targetCount != 0 && targetCount <= _counter)
            {
                _counter = 0;
                if (_skillModule == null)
                    _skillModule = _operator.GetModule<IAgentSkillModule>();
                
                _skillModule.UseSkill();
            }
        }

        public override bool TryTargeting()
            => targetCaster.SearchTargetSphere(agent.AgentStatusSO.DetectRadius);

        public override void UseSkill()
        {
            if (!isUsingSkill) return;
            
            if (_playerStateChange == null)
                _playerStateChange = _operator.PlayerStateChange;
            
            _playerStateChange.SendEventMessage(OperatorStateEnum.SKILL);
        }
    }
}