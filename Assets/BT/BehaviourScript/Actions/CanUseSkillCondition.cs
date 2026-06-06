using _Scripts.Agent;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CanUseSkill", story: "[Agent] can use skill", category: "Conditions", id: "87bcd55fe3b23518064feff520e73169")]
public partial class CanUseSkillCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Agent> Agent;

    private IAgentSkillModule _skillModule;
    
    public override bool IsTrue()
    {
        if(_skillModule == null) return false;
        
        return _skillModule.CanUseSkill();
    }

    public override void OnStart()
    {
        if (Agent == null || Agent.Value == null)
        {
            _skillModule = null;
            return;
        }
        _skillModule = Agent.Value.GetModule<IAgentSkillModule>();
    }
}
