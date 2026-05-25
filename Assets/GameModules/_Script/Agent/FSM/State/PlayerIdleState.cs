using _Script.Agent.FSM.Tags;
using _Script.ScriptableObject;
using UnityEngine;

namespace _Script.Agent.FSM.State
{
    public class PlayerIdleState : AbstractPlayerState, ICanDamageable
    {
        private int clipHash;
        public PlayerIdleState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
            clipHash = hash.AnimationHash;
        }

        public override void Enter()
        {
            base.Enter();
            _renderer.PlayAnimation(clipHash);
        }

        public override void Update()
        {
            base.Update();
            if (_playerOperator.touchedAgentList.Count != 0)
            {
                _playerOperator.ChangePlayerState(PlayerStateEnum.ATTACK);
                return;
            }
            if (TargetCaster.CastEnemy(_playerOperator.Collider))
            {
                _playerOperator.ChangePlayerState(PlayerStateEnum.ATTACK);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}