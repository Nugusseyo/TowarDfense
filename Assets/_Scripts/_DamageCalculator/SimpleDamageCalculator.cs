using UnityEngine;

[CreateAssetMenu(fileName = "SimpleDamageCalculator", menuName = "Scriptable Objects/SimpleDamageCalculator")]
public class SimpleDamageCalculator : DamageCalculator
{
    public override float CalculateDamage(float attackPower, float defensePower)
    {
        return Mathf.Max(attackPower - defensePower, 0f);
    }
}
