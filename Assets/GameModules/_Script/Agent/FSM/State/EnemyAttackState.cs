using _Script.Agent.Modules;
using _Script.Agent.Modules.BattleSystem;
using _Script.Agent.Modules.StatSystem;
using _Script.ScriptableObject;
using _Scripts.Agent;
using Agents.FSM;

namespace _Script.Agent.FSM.State
{
    public class EnemyAttackState : AbstractEnemyState
    {
        private IAnimationTrigger _trigger;
        private ISkillModule _skillModule;
        public EnemyAttackState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
            _trigger = agent.GetModule<IAnimationTrigger>();
            _skillModule = agent.GetModule<ISkillModule>();
        }

        public override void Enter()
        {
            base.Enter();
            _trigger.OnAttackTrigger += HandleEnemyAttack;
            _trigger.OnAnimationEnd += HandleEnemyAnimationEnd;
        }

        private void HandleEnemyAnimationEnd()
        {
            enemy.ChangeEnemyState(EnemyStateEnum.IDLE);
        }

        public override void Exit()
        {
            base.Exit();
            _trigger.OnAttackTrigger -= HandleEnemyAttack;
            _trigger.OnAnimationEnd -= HandleEnemyAnimationEnd;
        }

        private void HandleEnemyAttack()
        {
            if(enemy.holdingTarget != null)
                enemy.holdingTarget.GetDamage(_skillModule.GetDamageBase());
        }
    }
}