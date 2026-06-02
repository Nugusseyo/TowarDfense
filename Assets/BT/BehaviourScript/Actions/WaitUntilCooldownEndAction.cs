using _Scripts.Agent.Player;
using System;
using _Scripts.Agent;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Wait Until Cooldown End", story: "[Agent] wait until cooldown end", category: "Action/Combat", id: "3519429c8422de69d445b46496ea603b")]
public partial class WaitUntilCooldownEndAction : Action
{
    [SerializeReference] public BlackboardVariable<Agent> Agent;

    private float _startTime;
    private float _waitTime;
    
    protected override Status OnStart()
    {
        if (Agent.Value == null || Agent.Value.AgentStatusSO == null)
            return Status.Failure;
        _startTime = Time.time;
        _waitTime = Agent.Value.AgentStatusSO.NormalAttackCooldown;
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(_startTime + _waitTime < Time.time) return Status.Success;
        
        return Status.Running;
    }
}

