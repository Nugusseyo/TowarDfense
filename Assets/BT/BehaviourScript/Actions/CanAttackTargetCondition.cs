using _Scripts.Agent;
using System;
using GameModules._Script.Agent;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CanAttackTarget", story: "[Agent] can attack target", category: "Conditions", id: "3389a9147024d7c7971f810e6bf235de")]
public partial class CanAttackTargetCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Agent> Agent;

    private IAgentAttackModule _attackModule;
    
    public override bool IsTrue()
    {
        if (_attackModule == null) return false;
        
        return _attackModule.TryTargeting();
    }

    public override void OnStart()
    {
        if (Agent.Value == null)
            return;

        _attackModule = Agent.Value.GetModule<IAgentAttackModule>();
    }
}
