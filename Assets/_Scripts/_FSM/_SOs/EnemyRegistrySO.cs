using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRegistrySO", menuName = "Scriptable Objects/FSM/EnemyRegistrySO")]
public class EnemyRegistrySO : ScriptableObject
{
    private readonly List<EnemyObject> registeredEnemies = new();

    public void Register(EnemyObject enemy)
    {
        if (!registeredEnemies.Contains(enemy)) 
            registeredEnemies.Add(enemy);
    }

    public void Unregister(EnemyObject enemy)
    {
        if (registeredEnemies.Contains(enemy)) 
            registeredEnemies.Remove(enemy);
    }

    /*
     * 이 함수에서는 현재 등록된 적 오브젝트들 중에서
     * origin 오브젝트에 가장 가까운 것 과의 거리를 계산해 그 값을 반환해야 합니다.
     */
    public float GetClosestEnemyDistance(GameObject origin)
    {
        var closest = float.MaxValue;
        var originPos = origin.transform.position;
        
        foreach (var enemy in registeredEnemies)
        {
            var dist = Vector3.Distance(originPos, enemy.transform.position);
            if (dist < closest)
                closest = dist;
        }

        return closest;
    }
}