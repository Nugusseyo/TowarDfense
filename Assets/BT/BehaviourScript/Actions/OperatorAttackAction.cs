using _Scripts.Agent.Player;
using System;
using _Scripts.Agent;
using GameModules._Script.Agent;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Operator attack", story: "[Agent] attack in attackList", category: "Action/Combat", id: "966744992145ea5736d587e201f920ef")]
public partial class OperatorAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<Agent> Agent;

    private IAgentAttackModule _attackModule;
    private IAnimationTrigger _trigger;

    protected override Status OnStart()
    {
        if (Agent.Value == null)
            return Status.Failure;

        _trigger = Agent.Value.GetModule<IAnimationTrigger>();
        _attackModule = Agent.Value.GetModule<IAgentAttackModule>();
        
        if(_trigger == null || _attackModule == null) 
            return Status.Failure;
        
        _trigger.OnAttackTrigger += HandleAttackTrigger;

        return Status.Success;
    }

    private void HandleAttackTrigger()
    {
        _attackModule.AttackTarget();
        _trigger.OnAttackTrigger -= HandleAttackTrigger;
    }
}

