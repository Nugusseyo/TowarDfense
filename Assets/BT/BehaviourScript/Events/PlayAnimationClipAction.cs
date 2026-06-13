using _Script.ScriptableObject;
using System;
using _Scripts.Agent;
using _Scripts.Agent.Player;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlayAnimationClip", story: "[Agent] play animation [Clip] [During]", category: "Action/Animation", id: "7e89dcb7c4ed3b930e35739fb67622fd")]
public partial class PlayAnimationClipAction : Action
{
    [SerializeReference] public BlackboardVariable<Agent> Agent;
    [SerializeReference] public BlackboardVariable<AnimationHashSO> Clip;
    [SerializeReference] public BlackboardVariable<float> During;

    private IAgentRenderer _renderer;
    private Animator _animator;
    private float _timer;
    private float _duration;
    
    protected override Status OnStart()
    {
        if (Agent.Value == null || Clip.Value == null)
        {
            Debug.Log("기본값 누락");
            return Status.Failure;
        }
    
        _renderer = Agent.Value.GetModule<IAgentRenderer>();
        if (_renderer == null || _renderer.Animator == null)
        {
            Debug.Log("Animator 누락");
            return Status.Failure;
        }
        
        _renderer.PlayFadeAcrossClip(Clip.Value.AnimationHash, During.Value);
    
        _timer = 0f;
        _duration = During.Value; 
    
        return Status.Running;
    }
    protected override Status OnUpdate()
    {
        _timer += Time.deltaTime;
    
        if (_timer >= _duration)
        {
            return Status.Success;
        }
    
        return Status.Running;
    }
}

