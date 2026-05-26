using _Script.ScriptableObject;
using System;
using _Scripts.Agent.Player;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlayAnimationClip", story: "[Operator] play animation [Clip] [During]", category: "Action/Animation", id: "7e89dcb7c4ed3b930e35739fb67622fd")]
public partial class PlayAnimationClipAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractOperator> Operator;
    [SerializeReference] public BlackboardVariable<AnimationHashSO> Clip;
    [SerializeReference] public BlackboardVariable<float> During;

    protected override Status OnStart()
    {
        if (Operator.Value == null || Clip.Value == null || Operator.Value.Renderer == null)
            return Status.Failure;
        
        Operator.Value.Renderer.PlayFadeAcrossClip(Clip.Value.AnimationHash, During);
        return Status.Success;
    }
}

