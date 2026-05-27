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

    private Animator _animator;
    
    protected override Status OnStart()
    {
        if (Operator.Value == null || Clip.Value == null || Operator.Value.Renderer == null
            || Operator.Value.Renderer.Animator == null)
            return Status.Failure;
        _animator = Operator.Value.Renderer.Animator;
        AnimatorStateInfo animInfo;
        if (_animator.IsInTransition(0))
            animInfo = _animator.GetNextAnimatorStateInfo(0);
        else
            animInfo = _animator.GetCurrentAnimatorStateInfo(0);
        
        if(animInfo.shortNameHash != Clip.Value.AnimationHash) //똑같은거 또 하라고 하면 GET OUT
            Operator.Value.Renderer.PlayFadeAcrossClip(Clip.Value.AnimationHash, During);
        return Status.Success;
    }
}

