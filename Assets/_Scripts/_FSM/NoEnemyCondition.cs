using UnityEngine;
[CreateAssetMenu(fileName = "NoEnemyCondition", menuName = "Scriptable Objects/FSM/NoEnemyCondition")]
public class NoEnemyCondition : ConditionSO
{
    public float detectRadius = 15f;
    
    public override bool CheckCondition(TankStateMachine owner)
    {
        return owner.GetNearestEnemyDistance() > detectRadius;
    }
}