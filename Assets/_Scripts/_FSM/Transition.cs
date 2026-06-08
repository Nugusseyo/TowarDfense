using UnityEngine;
[System.Serializable]
public struct Transition
{
    public ConditionSO condition;
    public TankStateSO toState;
}