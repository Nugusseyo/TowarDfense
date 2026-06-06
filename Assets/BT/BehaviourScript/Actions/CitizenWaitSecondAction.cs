using _Scripts.Agent.Enemy;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Citizen Wait Second", story: "[Citizen] wait until setting second", category: "Action", id: "2f9c77e43057cdff5f0f32b035c819cb")]
public partial class CitizenWaitSecondAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractCitizen> Citizen;

    private NavMeshAgent _navAgent;
    private CitizenWayPoint _wayPoint;
    private float _startTime;
    
    protected override Status OnStart()
    {
        if (Citizen.Value == null) return Status.Failure;
        if (Citizen.Value.Mover == null) return Status.Failure;
        if (Citizen.Value.Mover.CurWayPoint == null) return Status.Failure;
        if (Citizen.Value.Mover.NavAgent == null) return Status.Failure;
        
        _wayPoint = Citizen.Value.Mover.CurWayPoint;
        _navAgent = Citizen.Value.Mover.NavAgent;
        _startTime = Time.time;

        _navAgent.isStopped = true;
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(_wayPoint.WaitSecond + _startTime < Time.time) return Status.Success;
        return Status.Running;
    }

    protected override void OnEnd()
    {
        base.OnEnd();
        if(_navAgent != null && _navAgent.isActiveAndEnabled && _navAgent.isOnNavMesh)
            _navAgent.isStopped = false;
    }
}

