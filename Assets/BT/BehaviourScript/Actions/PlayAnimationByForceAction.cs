using _Script.ScriptableObject;
using _Scripts.Agent.Player;
using System;
using _Script.Agent.Modules;
using _Scripts.Agent;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Play Animation By Force", story: "[Operator] play animation [Clip] by force", category: "Action/Animation", id: "217a16e34d3ceae74a77fde1c0c26b7f")]
public partial class PlayAnimationByForceAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractOperator> Operator;
    [SerializeReference] public BlackboardVariable<AnimationHashSO> Clip;

    private IAgentRenderer _renderer;
    
    protected override Status OnStart()
    {
        if (Operator.Value == null || Clip.Value == null)
            return Status.Failure;
        
        _renderer = Operator.Value.GetModule<IAgentRenderer>();
        
        _renderer.PlayClip(Clip.Value.AnimationHash);
        return Status.Success;
    }
}

