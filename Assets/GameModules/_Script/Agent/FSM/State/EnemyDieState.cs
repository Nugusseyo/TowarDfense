using _Script.ScriptableObject;

namespace _Script.Agent.FSM.State
{
    public class EnemyDieState : AbstractEnemyState
    {
        public EnemyDieState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
        }
    }
}