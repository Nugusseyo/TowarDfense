using _Scripts.Agent.Enemy;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Citizen Move to Next", story: "[Citizen] move to next position", category: "Action", id: "f6fb8bd6b63f9351a63df097a43fba02")]
public partial class CitizenMoveToNextAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractCitizen> Citizen;

    private ICitizenMover _mover;
    
    
    protected override Status OnStart()
    {
        if (Citizen.Value == null) return Status.Failure;
        _mover = Citizen.Value.Mover;
        if(_mover == null) return Status.Failure;
        
        _mover.SetNextPosition();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(_mover == null || Citizen.Value == null || _mover.NavAgent == null) return Status.Failure;
        
        return _mover.IsArrived ? Status.Success : Status.Running;
    }
}

