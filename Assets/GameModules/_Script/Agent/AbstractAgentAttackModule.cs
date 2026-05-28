using System.Collections.Generic;
using _Script.Agent.Modules;
using _Scripts.Agent;
using UnityEngine;
using UnityEngine.Events;

namespace GameModules._Script.Agent
{
    public abstract class AbstractAgentAttackModule : MonoBehaviour, IModule, IAgentAttackModule
    {
        public UnityEvent<Transform> OnAttack;
        
        protected _Scripts.Agent.Agent agent;
        protected ITargetCaster targetCaster;

        protected int _targetMaxCount;
        protected int _attackCount;
        
        protected readonly List<_Scripts.Agent.Agent> _attackTargets = new List<_Scripts.Agent.Agent>();
        public virtual void Initialize(ModuleAgent moduleAgent)
        {
            agent = moduleAgent as _Scripts.Agent.Agent;
            Debug.Assert(agent != null, $"AgentAttackModule은 Agent 전용입니다! Target : {gameObject.name}");

            targetCaster = agent.GetModule<ITargetCaster>();
            Debug.Assert(targetCaster != null, $"AttackModule은 TargetCaster가 필수입니다! Target : {gameObject.name}");
            
            Debug.Assert(agent.AgentStatusSO != null, $"AttackModule은 AgentStatusSO를 필요로 합니다. Target : {gameObject.name}");
            _targetMaxCount = agent.AgentStatusSO.MaxTargetCount;
            _attackCount = agent.AgentStatusSO.AttackCount;
        }

        public List<_Scripts.Agent.Agent> AttackTargetList => _attackTargets;

        public virtual void AttackTarget()
        {
            if (targetCaster.HitCount == 0) //분명 BT에서 타게팅을 미리 해줘야하는데. 안해준거임.
                return;

            SortingTargets();
            
            // AI : 실제 공격 실행 (리스트에 담긴 적 중 최대 _attackCount 만큼 타격)
            // AI : 몬스터가 1마리밖에 없는데 _attackCount가 3이라고 해서 에러가 나지 않도록 Mathf.Min 안전장치 추가
        }

        public void SortingTargets() //이거 SO로 따로 빼놓으면 좋을듯?
        {
            _attackTargets.Clear();
            // AI : 4. 거리를 정렬하기 위한 임시 리스트 (ValueTuple 활용)
            int hitCount = targetCaster.HitCount;
            Vector3 myPos = transform.position;
            List<(_Scripts.Agent.Agent agent, float sqrDist)> validTargets = new List<(_Scripts.Agent.Agent, float)>();
            //My : 튜플로 해주면 리스트에 거리와 Agent를 넣고, IComparable로 비교해 연산할 수 있다.
            //튜플은 구조체나 클래스를 귀찮게 쓰지 않고 여러개의 데이터를 사용할 수 있게 해주는 것.
            //validTargets.agent나 sqrDist로 해당 값을 꺼내올 수 있다.

            for (int i = 0; i < hitCount; i++)
            {
                Collider succeedCollider = targetCaster.SucceedColliders[i];
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

        public abstract bool TryTargeting();

        public void ChangeAttackTargetCount(int targetMaxCount, int attackCount)
        {
            _targetMaxCount = targetMaxCount;
            _attackCount = attackCount;
        }
    }
}
