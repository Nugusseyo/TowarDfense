using UnityEngine;

namespace _Scripts.Agent.Player
{
    [CreateAssetMenu(fileName = "PlayerStatus", menuName = "Agent/PlayerStatus")]
    public class OperatorStatusSO : ScriptableObject
    {
        [field: SerializeField] public float DetectRadius { get; private set; } = 10f;
        [field: SerializeField] public float StartAttackDelay { get; private set; } = 0.5f;
        [field: SerializeField] public float NormalAttackCooldown { get; private set; } = 0.6f;
        [field: SerializeField] public float SkillAttackCooldown { get; private set; } = 20f;
        [field: SerializeField] public int MaxTargetCount { get; private set; } = 2;
        [field: SerializeField] public int AttackCount { get; private set; } = 1;
        [field: SerializeField] public int Damage { get; private set; } = 200;
    }
}
