using _Scripts.Agent.Player;
using System;
using _Scripts.Agent;
using GameModules._Script.Agent;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Operator attack", story: "[Operator] attack in attackList", category: "Action/Combat", id: "966744992145ea5736d587e201f920ef")]
public partial class OperatorAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractOperator> Operator;

    private IAgentAttackModule _attackModule;
    private IAnimationTrigger _trigger;

    protected override Status OnStart()
    {
        if (Operator.Value == null || Operator.Value.Trigger == null)
            return Status.Failure;

        _attackModule = Operator.Value.GetModule<IAgentAttackModule>();
        
        if (_attackModule == null) 
            return Status.Failure;

        _trigger = Operator.Value.Trigger;
        _trigger.OnAttackTrigger += HandleAttackTrigger;

        return Status.Success;
    }

    private void HandleAttackTrigger()
    {
        _attackModule.AttackTarget();
        _trigger.OnAttackTrigger -= HandleAttackTrigger;
    }
}

