using UnityEngine;

[CreateAssetMenu(fileName = "HealthBelowCondition", menuName = "Scriptable Objects/FSM/HealthBelowCondition")]
public class HealthBelowCondition : ConditionSO
{
    public float threshold = 30f;
    
    public override bool CheckCondition(TankStateMachine owner)
    {
        return owner.CurrentHealth <= threshold;
    }
}