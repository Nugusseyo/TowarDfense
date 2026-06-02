using _Scripts.Agent;
using _Scripts.Feedbacks;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Agent Play Feedback", story: "[Agent] play [Feedback]", category: "Action", id: "eaacb5c540dd02c386762dbec54f1146")]
public partial class AgentPlayFeedbackAction : Action
{
    [SerializeReference] public BlackboardVariable<Agent> Agent;
    [SerializeReference] public BlackboardVariable<FeedbackType> Feedback;

    private Feedbacks _feedbacks;
    private IAnimationTrigger _trigger;
    
    protected override Status OnStart()
    {
        if (Agent.Value == null || Feedback.Value == null) return Status.Failure;

         _feedbacks = Agent.Value.GetModule<Feedbacks>();
         _trigger = Agent.Value.GetModule<IAnimationTrigger>();
         
        if(_feedbacks == null || _trigger == null) return Status.Failure;

        _trigger.OnAttackTrigger += HandlePlayFeedback;
        
        return Status.Success;
    }

    private void HandlePlayFeedback()
    {
        FeedbackPlayer player = _feedbacks.GetFeedbackPlayer(Feedback);
        if (player == null) return;
        
        player.FeedbackPlay();
        if(_trigger != null)
            _trigger.OnAttackTrigger -= HandlePlayFeedback;
        Debug.Log("Play Feedback!!");
    }
}

