using UnityEngine;

[CreateAssetMenu(fileName = "WaitTimeCondition", menuName = "Scriptable Objects/FSM/WaitTimeCondition")]
public class WaitTimeCondition : ConditionSO
{
    public float waitTime = 2f;
    
    public override bool CheckCondition(TankStateMachine owner)
    {
        return owner.StateTimer >= waitTime;
    }
}
