using System;
using _Script.Agent.Modules;
using _Scripts.Agent;
using _Scripts.Agent.Combat;
using _Scripts.Agent.Player;
using UnityEngine;

namespace GameModules._Script.Agent.Operator.AttackModule
{
    public class OperatorDropperAttackModule : AbstractAgentAttackModule
    {
        private PlayerStateChange _playerStateChange;
        private AbstractOperator _operator;

        [Header("Damage Settings")]
        [SerializeField] private float startDamagePercent = 0.05f; // 단위 : 1 = 100%
        [SerializeField] private float maxDamagePercent = 0.50f;   // 최대 50%까지 도달함.
        [SerializeField] private float targetTime = 40f;

        private float _lifeTime;
        
        // 💡 1 미만의 소수점 데미지들을 모아둘 누적 변수
        private float _damageAccumulator; 

        public override void Initialize(ModuleAgent moduleAgent)
        {
            base.Initialize(moduleAgent);
            
            _operator = agent as AbstractOperator;
        }

        private void Start()
        {
            _playerStateChange = _operator.PlayerStateChange;
            Debug.Assert(_playerStateChange != null, $"Operator에 StateChange Event가 존재하지 않습니다! Target : {gameObject.name}");
            
            AttackTarget();
            
            _lifeTime = 0f;
            _damageAccumulator = 0f; // 누적기 초기화
        }

        private void Update()
        {
            if (_operator.HealthModule.IsDead) return;
            
            // 생존 시간만 계속 누적해 줍니다.
            _lifeTime += Time.deltaTime;
            
            // 매 프레임 데미지를 계산합니다.
            ApplyContinuousDamage(Time.deltaTime);
        }
        
        private void ApplyContinuousDamage(float deltaTime)
        {
            HealthModule healthModule = _operator.HealthModule;
            if (healthModule == null) return;
            
            // 1. 현재 시간에 따른 초당 데미지 비율(%) 계산
            float timeRatio = Mathf.Clamp01(_lifeTime / targetTime);
            float currentDamagePercentPerSec = Mathf.Lerp(startDamagePercent, maxDamagePercent, timeRatio);
            
            // 2. 이번 '1프레임' 동안 들어갈 실수(float) 데미지 계산
            float damageThisFrame = (healthModule.MaxHealth * currentDamagePercentPerSec) * deltaTime;
            
            // 3. 누적기에 프레임 데미지 적립
            _damageAccumulator += damageThisFrame;
            
            // 4. 누적된 데미지가 1(정수) 이상이 되면 체력을 깎음
            if (_damageAccumulator >= 1f)
            {
                // 누적된 값에서 정수 부분만 추출
                int damageToApply = Mathf.FloorToInt(_damageAccumulator);
                
                // 적용한 데미지(정수)만큼 누적기에서 빼기 (소수점 찌꺼기는 다음 프레임으로 이월)
                _damageAccumulator -= damageToApply; 
                
                // 정수 데미지 적용
                healthModule.TakeDamage(damageToApply);
            }
        }

        public override void AttackTarget()
        {
            //일부로 부모꺼 안해준거임.
            targetCaster.SearchTargetSphere(_operator.AgentStatusSO.DetectRadius);
            SortingTargets();
            
            int actualHitCount = Mathf.Min(_attackTargets.Count, _attackCount);
            Debug.Log(actualHitCount + "명 때리기!");
            for (int i = 0; i < actualHitCount; ++i)
            {
                if (_attackTargets[i].TryGetComponent(out IHealable healable))
                {
                    healable.TakeHeal(_operator.AgentStatusSO.Damage);
                    OnAttack?.Invoke(_attackTargets[i].transform);
                    Debug.Log("야르띠");
                }
            }
        }

        public override bool TryTargeting()
            => false;

        public override void UseSkill()
        {
            
        }

        private void OnDrawGizmosSelected()
        {
            if (agent == null) return;
            if (agent.AgentStatusSO == null) return;
            
            Gizmos.color = Color.aquamarine;
            Gizmos.DrawWireSphere(transform.position, agent.AgentStatusSO.DetectRadius);
        }
    }
}