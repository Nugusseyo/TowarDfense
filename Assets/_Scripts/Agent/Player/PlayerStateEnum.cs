using Unity.Behavior;

namespace _Scripts.Agent.Player
{
    [BlackboardEnum]
    public enum OperatorStateEnum
    {
        APPEAR,
        IDLE,
        ATTACK,
        SKILL,
        DEAD
    }
}
