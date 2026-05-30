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
    [NodeDescription(name: "WaitUntilAnimationEnd", story: "[Operator] wait animation end", category: "Action/Animation", id: "06d0f8d8f9c0b2c199cf93c4b9e757ac")]
    public partial class WaitUntilAnimationEndAction : Action
    {
        [SerializeReference] public BlackboardVariable<AbstractOperator> Operator;

        private IAnimationTrigger _trigger;
        private bool _isAnimationEnd = false;
        protected override Status OnStart()
        {
            if (Operator.Value == null)
            {
                Debug.LogError("Operator is Null!!!");
                return Status.Failure;
            }
        
            _trigger = Operator.Value.GetModule<IAnimationTrigger>();
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
            if (_isAnimationEnd)
            {
                return Status.Success;
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

