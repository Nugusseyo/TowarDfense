using _Scripts.Agent.Player;
using System;
using _Scripts.Agent;
using GameModules._Script.Agent;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FindTarget", story: "[Agent] find target", category: "Action/Combat", id: "d49981c9cabc49dacdd74e804c6f55a6")]
public partial class FindTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<Agent> Agent;

    private IAgentAttackModule _attackModule;
    
    protected override Status OnStart()
    {
        if (Agent.Value == null)
            return Status.Failure;

        _attackModule = Agent.Value.GetModule<IAgentAttackModule>();
        
        return _attackModule.TryTargeting() ? Status.Success : Status.Failure;
    }
}

