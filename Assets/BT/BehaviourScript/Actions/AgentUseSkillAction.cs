using _Scripts.Agent;
using System;
using _Script.Agent.Modules.BattleSystem;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Agent Use Skill", story: "[Agent] use skill", category: "Action/Combat", id: "d02a63f48d38c85a62e529f70fdd6be7")]
public partial class AgentUseSkillAction : Action
{
    [SerializeReference] public BlackboardVariable<Agent> Agent;
    
    private IAgentSkillModule _skillModule;
    private IAnimationTrigger _trigger;

    protected override Status OnStart()
    {
        if (Agent.Value == null) return Status.Failure;
        
        _skillModule = Agent.Value.GetModule<IAgentSkillModule>();
        _trigger = Agent.Value.GetModule<IAnimationTrigger>();
        
        if(_skillModule == null || _trigger == null) return Status.Failure;

        _trigger.ResetAttackTrigger();
        _trigger.OnAttackTrigger += HandleOperatorUseSkill;

        _skillModule.UseSkillStart();

        return Status.Success;
    }

    private void HandleOperatorUseSkill()
    {
        _skillModule.UseSkill();
        _skillModule.IsUsingSkill = false;
        _trigger.OnAttackTrigger -= HandleOperatorUseSkill;
    }
}

