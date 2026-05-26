using _Scripts.Agent.Player;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Wait Until Cooldown End", story: "[Operator] wait until cooldown end", category: "Action/Combat", id: "3519429c8422de69d445b46496ea603b")]
public partial class WaitUntilCooldownEndAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractOperator> Operator;

    private float _startTime;
    private float _waitTime;
    
    protected override Status OnStart()
    {
        if (Operator.Value == null || Operator.Value.AgentStatusSO == null)
            return Status.Failure;
        _startTime = Time.time;
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(_startTime + _waitTime < Time.time) return Status.Success;
        
        return Status.Running;
    }
}

