using System;
using _Script.Agent.FSM.Tags;
using _Script.Agent.Modules;
using _Script.ScriptableObject;

namespace _Script.Agent.FSM.State
{
    public class PlayerStartState : AbstractPlayerState, ICanDamageable
    {
        public PlayerStartState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _renderer.PlayAnimation(_animationHash.AnimationHash);
            _trigger.OnAnimationEnd += HandleOperatorSpawned;
        }

        private void HandleOperatorSpawned()
        {
            _playerOperator.ChangePlayerState(PlayerStateEnum.IDLE);
            _playerOperator.HealthBarSetActive(true);
        }

        public override void Exit()
        {
            base.Exit();
            _trigger.OnAnimationEnd -= HandleOperatorSpawned;
        }
    }
}