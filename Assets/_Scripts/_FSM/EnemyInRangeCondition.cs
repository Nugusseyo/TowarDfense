using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInRangeCondition", menuName = "Scriptable Objects/FSM/EnemyInRangeCondition")]
public class EnemyInRangeCondition : ConditionSO
{
    public float detectRadius = 8f;
    
    public override bool CheckCondition(TankStateMachine owner)
    {
        return owner.GetNearestEnemyDistance() <= detectRadius;
    }
}