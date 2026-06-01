using System.Collections.Generic;
using _Script.Agent.Modules;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.Agent.Enemy
{
    public class CitizenMover : MonoBehaviour, IModule, ICitizenMover
    {
        [SerializeField] private Vector2 offset;

        public NavMeshAgent NavAgent { get; private set; }
        private Agent _agent;

        public bool IsArrived
        {
            get
            {
                if (NavAgent == null || !NavAgent.isActiveAndEnabled || !NavAgent.isOnNavMesh)
                    return false;
                
                if (NavAgent.pathPending)
                    return false;
                
                return NavAgent.stoppingDistance >= NavAgent.remainingDistance;
            }
        }

        public int curIndex = 0;
        public CitizenWayPoint CurWayPoint
        {
            get
            {
                if (_citizenWayPoints == null || _citizenWayPoints.Count == 0) return null;
                int activeIndex = Mathf.Clamp(curIndex - 1, 0, _citizenWayPoints.Count - 1); // 1 빼줘야댐. 현재 인덱스라
                return _citizenWayPoints[activeIndex];
            }
        }
        
        private List<CitizenWayPoint> _citizenWayPoints;
        public void Initialize(ModuleAgent moduleAgent)
        {
            _agent = moduleAgent as Agent;
            
            NavAgent = moduleAgent.GetComponent<NavMeshAgent>();
            Debug.Assert(NavAgent != null, $"Citizen은 NavAgent가 필수입니다!");
        }

        public void EnemyInitialize(List<CitizenWayPoint> citizenWayPoints)
        {
            _citizenWayPoints = citizenWayPoints;
            curIndex = 0;
        }

        public void SetNextPosition()
        {
            if (_citizenWayPoints.Count <= curIndex)
            {
                curIndex = _citizenWayPoints.Count - 1;
                ArrivedEndPoint();
                return;
            }
            CitizenWayPoint wayPoint = _citizenWayPoints[curIndex];

            NavAgent.SetDestination(wayPoint.TargetPos);
            curIndex++;
        }

        public void ArrivedEndPoint()
        {
            Debug.Log("도착함!!");
            
            if (NavAgent != null)
            {
                NavAgent.enabled = false;
            }

            Destroy(_agent.gameObject);
        }
        
        
        

        [ContextMenu("RandomOffset")]
        public void RandomOffset()
        {
            offset = new Vector2(Random.Range(-1, 1f), Random.Range(-1, 1f));
        }
    }

    public class CitizenWayPoint
    {
        public float WaitSecond;
        public Vector3 TargetPos;
    }
}
