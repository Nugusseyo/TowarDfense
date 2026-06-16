using Unity.Behavior;

namespace _Scripts.Agent.Tower
{
    [BlackboardEnum]
    public enum TowerState
    {
        APPEAR,
        IDLE,
        RELOAD,
        FIRE,
        SHUTDOWN,
        DEAD
    }
}