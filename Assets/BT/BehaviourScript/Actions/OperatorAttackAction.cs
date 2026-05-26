using _Scripts.Agent.Player;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Operator attack", story: "[Operator] attack in attackList", category: "Action/Combat", id: "966744992145ea5736d587e201f920ef")]
public partial class OperatorAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractOperator> Operator;
    
    

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

