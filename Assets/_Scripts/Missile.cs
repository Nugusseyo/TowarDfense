using UnityEngine;

public class Missile : MonoBehaviour
{
    public float missileAttackPower = 10f;
    
    public DamageCalculator damageCalculator;

    private void OnCollisionEnter(Collision collision)
    {
        var target = collision.gameObject.GetComponent<BaseTankUnit>();
        if (target)
        {
            // 1. 기본 변수 대입 방식
            //var damage = missileAttackPower - target.defensePower;
            
            // 2. 데미지 계산기 방식
            var damage = damageCalculator.CalculateDamage(missileAttackPower, target.defensePower);
            
            // Debug.Log($"대상: {collision.gameObject.name}, 데미지: {damage}");
            
            // 실제로 탱크에게 데미지를 줌
            target.TakeDamage(damage);
        }
        else
        {
            Debug.Log("대상이 탱크가 아님");
        }

        Destroy(gameObject);
    }
}