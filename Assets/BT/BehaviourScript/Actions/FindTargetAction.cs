using _Scripts.Agent.Player;
using System;
using _Scripts.Agent;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindTarget", story: "[Agent] find target", category: "Action/Combat", id: "d49981c9cabc49dacdd74e804c6f55a6")]
public partial class FindTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<Agent> Agent;

    private ITargetCaster _sensor;
    
    protected override Status OnStart()
    {
        if (Agent.Value == null || Agent.Value.TargetCaster == null)
            return Status.Failure;
        
        _sensor = Agent.Value.TargetCaster;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Agent.Value.TryCasting() ? Status.Success : Status.Failure;
    }
}

