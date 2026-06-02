using _Scripts.Agent.Player;
using System;
using _Scripts.Agent;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Wait Delay before attack", story: "[Agent] wait before attack", category: "Action", id: "ae7982695b07e71b5fd16b0ea7784982")]
public partial class WaitDelayBeforeAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<Agent> Agent;

    private float _startTime;
    private float _waitTime;
    
    protected override Status OnStart()
    {
        if (Agent.Value == null || Agent.Value.AgentStatusSO == null)
            return Status.Failure;

        _startTime = Time.time;
        _waitTime = Agent.Value.AgentStatusSO.StartAttackDelay;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_startTime + _waitTime < Time.time) return Status.Success;

        return Status.Running;
    }
}

