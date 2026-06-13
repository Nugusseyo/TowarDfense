using System;
using _Scripts.Agent;
using _Scripts.Agent.Player;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace BT.BehaviourScript.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "WaitUntilAnimationEnd", story: "[Agent] wait animation end", category: "Action/Animation", id: "06d0f8d8f9c0b2c199cf93c4b9e757ac")]
    public partial class WaitUntilAnimationEndAction : Action
    {
        [SerializeReference] public BlackboardVariable<Agent> Agent;

        private IAnimationTrigger _trigger;
        private Animator _animator;
        private bool _isAnimationEnd = false;
        
        protected override Status OnStart()
        {
            if (Agent.Value == null)
            {
                Debug.LogError("Operator is Null!!!");
                return Status.Failure;
            }
        
            _trigger = Agent.Value.GetModule<IAnimationTrigger>();
            
            _animator = Agent.Value.GetModule<AgentRenderer>().Animator; 

            if (_trigger == null)
            {
                Debug.LogError("Trigger is Null!!!");
                return Status.Failure;
            }
            
            _trigger.ResetEndTrigger();
            _isAnimationEnd = false;
            _trigger.OnAnimationEnd += HandleAnimationEnd;
        
            return Status.Running;
        }

        private void HandleAnimationEnd() => _isAnimationEnd = true;

        protected override Status OnUpdate()
        {
            if (_animator != null && _isAnimationEnd)
            {
                AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.normalizedTime >= 1.0f && !_animator.IsInTransition(0))
                {
                    //normalizedTime이 1 이상이면 끝난거임.
                    return Status.Success;
                }
            }
        
            return Status.Running;
        }

        protected override void OnEnd()
        {
            if(_trigger != null)
                _trigger.OnAnimationEnd -= HandleAnimationEnd;
        }
    }
}