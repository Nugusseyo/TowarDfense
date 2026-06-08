using UnityEngine;

public class BaseTankUnit : MonoBehaviour
{
    public AttackPattern currentAttackPattern;
    public Transform firePoint;
    public float defensePower = 5f;
    
    // 2
    [SerializeField]
    private float maxHP = 25f;
    protected float currentHP;
    
    // 3 
    public TankDestroyedEvent OnTankDestroyed;
    
    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log($"{gameObject.name}이(가) {damage}만큼 피해를 입었습니다. 남은 HP: {currentHP}/{maxHP}");
        if (currentHP <= 0f)
        {
            currentHP = 0f;
            Debug.Log($"{gameObject.name}이(가) 파괴되었습니다!");
            
            // 3
            if (OnTankDestroyed != null)
                OnTankDestroyed.Raise(this);
            
            Destroy(gameObject);
        }
    }
    
    // 2
    protected void Attack()
    {
        if (currentAttackPattern&& firePoint)
            currentAttackPattern.ExecuteAttack(firePoint);
        else
            Debug.LogWarning("공격 패턴 또는 firePoint가 할당되지 않았습니다!");
    }
}