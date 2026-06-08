using System.Collections;
using UnityEngine;

public class AITankUnit : BaseTankUnit
{
    public float attackInterval = 2.0f;

    private void Start()
    {
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);
            Attack();
        }
    }
}