using System;
using System.Collections.Generic;
using _Script.Agent.FSM;
using _Script.Agent.Modules.BattleSystem;
using _Script.Agent.Modules.StatSystem;
using _Script.ScriptableObject;
using _Script.ScriptableObject.Event;
using GameLib.PoolObject.Runtime;
using GameLib.SoundSystem;
using UnityEngine;
using UnityEngine.Events;

namespace _Script.Agent.Operator
{
    public class PlayerOperator : _Script.Agent.Agent, IPoolable
    {
        public List<Agent> touchedAgentList = new List<Agent>();
        [field:SerializeField] public int MaxTouchCount { get; private set; }
        [field:SerializeField] public int MaxAttackCount { get; private set; }
        [SerializeField] private GameObject healthBar;

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void AfterInitialize()
        {
            base.AfterInitialize();
        }

        protected override void HandleHealthChaged(float prevHealth, float currentHealth, float max)
        {
            if (currentHealth <= 0 && !IsDead)
            {
                OperatorDeath();
            }
        }
    /// <summary>
    /// 웬만해서는 base.OperatorDeath보다 하고싶은 활동을 먼저 할 것.
    /// TouchedAgentList 초기화가 포함되어있음.
    /// </summary>
        protected virtual void OperatorDeath()
        {
            IsDead = true;
            ChangePlayerState(PlayerStateEnum.DIE);
        }

        public override void GetDamage(DamageData damageData)
        {
            base.GetDamage(damageData);
            float attackValue = damageData.Amount - _skillModule.GetDefensiveValue();
            if (attackValue <= 100) attackValue = 100;
            Health.GetDamage(attackValue);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent<Enemy.Enemy>(out Enemy.Enemy enemy))
            {
                if (touchedAgentList.Count < MaxTouchCount && enemy.holdingTarget == null)
                {
                    Debug.Log($"Block Enemy! {enemy.name}");
                    touchedAgentList.Add(enemy);
                    enemy.holdingTarget = this;
                    enemy.StartEnemyHolding(this);
                }
            }
        }

        public void HealthBarSetActive(bool isActive) => healthBar.SetActive(isActive);

        public void PlayerPull() => Destroy(gameObject);
        [field:SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => this == null ? null : gameObject;
        public void ResetItem()
        {
            Health.ResetHealth();
            _statModule.ClearBuff();
        }
    }
}