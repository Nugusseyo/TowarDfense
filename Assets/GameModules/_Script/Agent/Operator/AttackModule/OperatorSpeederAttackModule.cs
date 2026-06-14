using System.Collections;
using System.Collections.Generic;
using _Script.Agent.Modules;
using _Scripts.Agent.Player;
using UnityEngine;
using UnityEngine.AI;

namespace GameModules._Script.Agent.Operator.AttackModule
{
    public class OperatorSpeederAttackModule : AbstractAgentAttackModule, IModule
    {
        private PlayerStateChange _playerStateChange;
        private AbstractOperator _operator;

        [Header("Speed Buff Settings")]
        [SerializeField] private float speedIncreaseAmount = 1f;
        [SerializeField] private float buffDuration = 1f;
        
        private Dictionary<NavMeshAgent, Coroutine> _speedBuffCoroutines = new Dictionary<NavMeshAgent, Coroutine>();

        public override void Initialize(ModuleAgent moduleAgent)
        {
            base.Initialize(moduleAgent);
            _operator = agent as AbstractOperator;
        }

        public override bool TryTargeting()
            => targetCaster.SearchTargetSphere(agent.AgentStatusSO.DetectRadius);

        public override void AttackTarget()
        {
            base.AttackTarget();
            
            int actualHitCount = Mathf.Min(_attackTargets.Count, _attackCount);
            for (int i = 0; i < actualHitCount; ++i)
            {
                if (_attackTargets[i].TryGetComponent(out NavMeshAgent nav))
                {
                    ApplyOrRefreshSpeedBuff(nav);
                    
                    OnAttack?.Invoke(_attackTargets[i].transform);
                }
            }
        }

        public override void UseSkill()
        {
            if (_playerStateChange == null)
                _playerStateChange = _operator.PlayerStateChange;
            
            _playerStateChange.SendEventMessage(OperatorStateEnum.SKILL);
            Debug.Log("Use Skill!!!");
        }
        
        private void ApplyOrRefreshSpeedBuff(NavMeshAgent nav)
        {
            if (_speedBuffCoroutines.TryGetValue(nav, out Coroutine existingCoroutine))
            {
                if (existingCoroutine != null)
                {
                    StopCoroutine(existingCoroutine);
                }
            }
            else
            {
                nav.speed += speedIncreaseAmount;
            }
            
            _speedBuffCoroutines[nav] = StartCoroutine(RemoveSpeedBuffRoutine(nav));
        }
        
        private IEnumerator RemoveSpeedBuffRoutine(NavMeshAgent nav)
        {
            yield return new WaitForSeconds(buffDuration);
            
            if (nav != null)
            {
                nav.speed -= speedIncreaseAmount;
            }
            _speedBuffCoroutines.Remove(nav);
        }
        
        private void OnDisable()
        {
            foreach (KeyValuePair<NavMeshAgent, Coroutine> kvp in _speedBuffCoroutines)
            {
                if (kvp.Key != null)
                {
                    kvp.Key.speed -= speedIncreaseAmount;
                }
            }
            _speedBuffCoroutines.Clear();
        }
    }
}