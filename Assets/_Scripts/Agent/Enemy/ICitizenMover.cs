using System.Collections.Generic;
using UnityEngine.AI;

namespace _Scripts.Agent.Enemy
{
    public interface ICitizenMover
    {
        bool IsArrived { get; }
        CitizenWayPoint CurWayPoint { get; }
        NavMeshAgent NavAgent { get; }
        void EnemyInitialize(List<CitizenWayPoint> citizenWayPoints);
        void SetNextPosition();
        void ArrivedEndPoint();
        void RandomOffset();
    }
}