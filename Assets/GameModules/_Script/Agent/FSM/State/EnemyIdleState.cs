using _Script.ScriptableObject;
using Agents.FSM;

namespace _Script.Agent.FSM.State
{
    public class EnemyIdleState : AbstractEnemyState
    {
        public EnemyIdleState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            if(enemy.holdingTarget == null)
                enemy.StartEnemyMove();
            //else if(Vector3)
            {
                enemy.ChangeEnemyState(EnemyStateEnum.ATTACK);
            }
        }

        public override void Update()
        {
            base.Update();
            if (enemy.holdingTarget)
            {
                enemy.ChangeEnemyState(EnemyStateEnum.ATTACK);
            }
        }
    }
}