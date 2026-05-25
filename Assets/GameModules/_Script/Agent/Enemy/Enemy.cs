using System.Collections;
using System.Collections.Generic;
using _Script.Agent.Modules.BattleSystem;
using _Script.Agent.Modules.StatSystem;
using _Script.Agent.Operator;
using Agents.FSM;
using UnityEngine;
using UnityEngine.AI;

namespace _Script.Agent.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : Agent, IDamageable
    {
        public Transform spawnTrm;
        
        private NavMeshAgent _navMeshAgent;

        public List<EnemyRoute> enemyRouteList = new List<EnemyRoute>();

        private IStatModule _statModule;

        public PlayerOperator holdingTarget;

        protected override void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            base.Awake();
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            _statModule = GetModule<IStatModule>();
            Debug.Assert(_statModule != null, $"{gameObject.name}의 StatModule이 존재하지 않습니다!");
            
        }

        protected override void AfterInitialize()
        {
            base.AfterInitialize();
        }

        private void Start()
        {
            transform.position = new Vector3(spawnTrm.position.x, 1.5f,  spawnTrm.position.z);
            Collider.enabled = true;
            ChangeEnemyState(EnemyStateEnum.IDLE);
        }

        private IEnumerator EnemyMove()
        {
            if(_navMeshAgent.isStopped)
                _navMeshAgent.isStopped = false;
            List<EnemyRoute> followPosition = new List<EnemyRoute>(enemyRouteList);
            foreach (EnemyRoute enemyRoute in followPosition)
            {
                Vector2 targetPosition = new Vector2(enemyRoute.moveTransform.position.x + enemyRoute.offset.x,
                    enemyRoute.moveTransform.position.z + enemyRoute.offset.z);
                _navMeshAgent.SetDestination(enemyRoute.moveTransform.position + enemyRoute.offset);
                while (true)
                {
                    Vector2 transformPosition = new Vector2(transform.position.x, transform.position.z);
                    if (Vector2.Distance(targetPosition, transformPosition) < 0.5f)
                    {
                        enemyRouteList.Remove(enemyRoute);
                        break;
                    }
                    yield return null;
                }
                if(enemyRouteList.Count == 0) Debug.Log($"목적지에 도달했음. {gameObject.name}");
                yield return new WaitForSeconds(enemyRoute.waitSecond);
            }
        }

        public void StartEnemyHolding(PlayerOperator playerOperator)
        {
            StopCoroutine(EnemyMove());
            _navMeshAgent.isStopped = true;
        }

        public void StartEnemyMove() => StartCoroutine(EnemyMove());

        protected override void HandleHealthChaged(float prevHealth, float currentHealth, float max)
        {
            if (currentHealth <= 0 && !IsDead)
            {
                EnemyDeath();
            }
        }

        protected virtual void EnemyDeath()
        {
            if (holdingTarget != null)
                holdingTarget.touchedAgentList.Remove(this);
            StopAllCoroutines();
            IsDead = true;
            Destroy(gameObject);
            OnDeath?.Invoke();
        }

        public override void GetDamage(DamageData damageData)
        {
            base.GetDamage(damageData);
            float attackValue = damageData.Amount - _skillModule.GetDefensiveValue();
            if (attackValue <= 100) attackValue = 100;
            Health.GetDamage(attackValue);
        }
    }
}
