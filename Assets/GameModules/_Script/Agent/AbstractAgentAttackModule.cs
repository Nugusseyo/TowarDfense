using System.Collections.Generic;
using _Script.Agent.Modules;
using _Scripts.Agent;
using _Scripts.Agent.Player;
using GameModules._Script.Agent.CombatSystem.SortingSystem;
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

        [field: SerializeField] public SortingTargetSO SortingRule { get; private set; }

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
        }

        public void SortingTargets() //이거 SO로 따로 빼놓으면 좋을듯?
        {
            _attackTargets.Clear();
            //int hitCount = targetCaster.HitCount;
            //Vector3 myPos = transform.position;
            //List<(_Scripts.Agent.Agent agent, float sqrDist)> validTargets = new List<(_Scripts.Agent.Agent, float)>();
            //My : 튜플로 해주면 리스트에 거리와 Agent를 넣고, IComparable로 비교해 연산할 수 있다.
            //튜플은 구조체나 클래스를 귀찮게 쓰지 않고 여러개의 데이터를 사용할 수 있게 해주는 것.
            //validTargets.agent나 sqrDist로 해당 값을 꺼내올 수 있다.

            if (SortingRule == null)
            {
                Debug.LogError("Sorting Rule이 존재하지 않습니다.");
                return;
            }
            
            SortingRule.SortingTarget(targetCaster.SucceedColliders, targetCaster.HitCount, transform.position, _targetMaxCount, _attackTargets);
            /*
            for (int i = 0; i < hitCount; i++)
            {
                Collider succeedCollider = targetCaster.SucceedColliders[i];
                if (succeedCollider == null) continue;

                if (succeedCollider.TryGetComponent<_Scripts.Agent.Agent>(out var targetAgent))
                {
                    // Distance 대신 제곱 비교하면 더 좋음. 야르 ㅋ
                    float sqrDistance = (targetAgent.transform.position - myPos).sqrMagnitude;
                    validTargets.Add((targetAgent, sqrDistance));
                }
            }
            
            validTargets.Sort((a, b) => a.sqrDist.CompareTo(b.sqrDist));
            //My : Sorting할 때, 비교군 sqrDist를 가져와서 둘이 서로 비교한다.
            //위 식은 오름차순. b.sqrDist.CompareTo(a.sqrDist) = 내림차순.
            
            int targetCountToTake = Mathf.Min(validTargets.Count, _targetMaxCount);
            for (int i = 0; i < targetCountToTake; i++)
            {
                _attackTargets.Add(validTargets[i].agent);
            }
            */
        }

        public abstract bool TryTargeting();
        public abstract void UseSkill();

        public void ChangeAttackTargetCount(int targetMaxCount, int attackCount)
        {
            _targetMaxCount = targetMaxCount;
            _attackCount = attackCount;
        }
    }
}
