using _Script.Agent.FSM.State;
using _Script.ScriptableObject;
using UnityEngine;

namespace _Script.Agent.FSM
{
    public class PlayerDieState : AbstractPlayerState
    {
        private int clipHash;
        public PlayerDieState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
            clipHash = hash.AnimationHash;
        }

        public override void Enter()
        {
            base.Enter();
            _playerOperator.HealthBarSetActive(false);
            
            foreach (Agent enemy in _playerOperator.touchedAgentList)
            {
                Enemy.Enemy targetEnemy = enemy as Enemy.Enemy;
                targetEnemy.holdingTarget = null;
            }
            _playerOperator.touchedAgentList.Clear();
            
            _renderer.PlayAnimation(clipHash);
            _trigger.OnAnimationEnd += HandleOperatorDieAnimationEnd;
        }

        private void HandleOperatorDieAnimationEnd()
        {
            
            _playerOperator.OnDeath?.Invoke();
            _playerOperator.PlayerPull();
        }

        public override void Exit()
        {
            base.Exit();

            _trigger.OnAnimationEnd -= HandleOperatorDieAnimationEnd;
        }
    }
}