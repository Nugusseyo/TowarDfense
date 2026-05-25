using _Script.ScriptableObject;

namespace _Script.Agent.FSM.State
{
    public class AbstractEnemyState : AgentState
    {
        protected Enemy.Enemy enemy;
        protected int clipHash;
        public AbstractEnemyState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
            enemy = agent as Enemy.Enemy;
            clipHash = hash.AnimationHash;
        }

        public override void Enter()
        {
            base.Enter();
            
            _renderer.PlayAnimation(clipHash);
        }
    }
}