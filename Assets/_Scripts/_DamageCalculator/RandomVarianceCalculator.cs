using UnityEngine;

[CreateAssetMenu(fileName = "RandomVarianceCalculator", menuName = "Scriptable Objects/RandomVarianceCalculator")]
public class RandomVarianceCalculator : DamageCalculator
{
    public float minMultiplier = 0.8f;
    public float maxMultiplier = 1.2f;

    public override float CalculateDamage(float attackPower, float defensePower)
    {
        var baseDamage = Mathf.Max(attackPower - defensePower, 0f);
        var variance = Random.Range(minMultiplier, maxMultiplier);
        return baseDamage * variance;
    }
}