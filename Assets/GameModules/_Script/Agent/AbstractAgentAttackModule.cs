using System.Collections.Generic;
using _Script.Agent.Modules;
using _Scripts.Agent;
using UnityEngine;

namespace GameModules._Script.Agent
{
    public abstract class AbstractAgentAttackModule : MonoBehaviour, IModule, IAgentAttackModule
    {
        private _Scripts.Agent.Agent _agent;
        private ITargetCaster _targetCaster;

        private int _targetMaxCount;
        private int _attackCount;
        
        private readonly List<_Scripts.Agent.Agent> _attackTargets = new List<_Scripts.Agent.Agent>();
        public void Initialize(ModuleAgent moduleAgent)
        {
            _agent = moduleAgent as _Scripts.Agent.Agent;
            Debug.Assert(_agent != null, $"AgentAttackModule은 Agent 전용입니다! Target : {gameObject.name}");

            _targetCaster = _agent.GetModule<ITargetCaster>();
            Debug.Assert(_targetCaster != null, $"AttackModule은 TargetCaster가 필수입니다! Target : {gameObject.name}");
            
            Debug.Assert(_agent.AgentStatusSO != null, $"AttackModule은 AgentStatusSO를 필요로 합니다. Target : {gameObject.name}");
            _targetMaxCount = _agent.AgentStatusSO.MaxTargetCount;
            _attackCount = _agent.AgentStatusSO.AttackCount;

        }

        public List<_Scripts.Agent.Agent> AttackTargetList => _attackTargets;

        public virtual void AttackTarget()
        {
            if (_targetCaster.SucceedColliders[0] == null) //분명 BT에서 타게팅을 미리 해줘야하는데. 안해준거임.
                return;

            SortingTargets();
            
            // AI : 실제 공격 실행 (리스트에 담긴 적 중 최대 _attackCount 만큼 타격)
            // AI : 몬스터가 1마리밖에 없는데 _attackCount가 3이라고 해서 에러가 나지 않도록 Mathf.Min 안전장치 추가
            int actualHitCount = Mathf.Min(_attackTargets.Count, _attackCount);
            Debug.Log($"{actualHitCount}, {_attackTargets.Count}, {_attackCount}");
            for (int i = 0; i < actualHitCount; ++i)
            {
                Debug.Log($"{_agent.name}이(가) 범위 내 가장 가까운 적인 [{_attackTargets[i].name}]을(를) 때리기!");
                // AI : 여기서 실제 데미지를 주는 로직(예: _attackTargets[i].TakeDamage(...))을 실행하시면 됩니다.
            }
        }

        public void SortingTargets()
        {
            _attackTargets.Clear();
            // AI : 4. 거리를 정렬하기 위한 임시 리스트 (ValueTuple 활용)
            int hitCount = _targetCaster.HitCount;
            Vector3 myPos = transform.position;
            List<(_Scripts.Agent.Agent agent, float sqrDist)> validTargets = new List<(_Scripts.Agent.Agent, float)>();
            //My : 튜플로 해주면 리스트에 거리와 Agent를 넣고, IComparable로 비교해 연산할 수 있다.
            //튜플은 구조체나 클래스를 귀찮게 쓰지 않고 여러개의 데이터를 사용할 수 있게 해주는 것.
            //validTargets.agent나 sqrDist로 해당 값을 꺼내올 수 있다.

            for (int i = 0; i < hitCount; i++)
            {
                Collider succeedCollider = _targetCaster.SucceedColliders[i];
                if (succeedCollider == null) continue;

                // AI : 콜라이더에서 Agent 컴포넌트를 안전하게 추출
                if (succeedCollider.TryGetComponent<_Scripts.Agent.Agent>(out var targetAgent))
                {
                    // AI : Vector3.Distance 대신 제곱비교(sqrMagnitude)를 쓰면 루트 연산이 생략되어 성능이 대폭 향상됩니다.
                    float sqrDistance = (targetAgent.transform.position - myPos).sqrMagnitude;
                    validTargets.Add((targetAgent, sqrDistance));
                }
            }

            // AI : 거리가 가까운 순서대로 오름차순 정렬
            validTargets.Sort((a, b) => a.sqrDist.CompareTo(b.sqrDist));
            //My : Sorting할 때, 비교군 sqrDist를 가져와서 둘이 서로 비교한다.
            //위 식은 오름차순. b.sqrDist.CompareTo(a.sqrDist) = 내림차순.

            // AI : 최대 타겟팅 수량(_targetMaxCount)만큼만 _attackTargets에 적재
            int targetCountToTake = Mathf.Min(validTargets.Count, _targetMaxCount);
            Debug.Log($"TargetTake : {targetCountToTake}");
            for (int i = 0; i < targetCountToTake; i++)
            {
                _attackTargets.Add(validTargets[i].agent);
            }
        }

        public void ChangeAttackTargetCount(int targetMaxCount, int attackCount)
        {
            _targetMaxCount = targetMaxCount;
            _attackCount = attackCount;
        }
    }
}
