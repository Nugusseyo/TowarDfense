using UnityEngine;

public abstract class DamageCalculator : ScriptableObject
{
    public abstract float CalculateDamage(float attackPower, float defensePower);
}