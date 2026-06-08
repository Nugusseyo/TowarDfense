using UnityEngine;

[CreateAssetMenu(fileName = "SingleShotAttack", menuName = "Scriptable Objects/SingleShotAttack")]
public class SingleShotAttack : AttackPattern
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    public override void ExecuteAttack(Transform firePoint)
    {
        var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        var rb = projectile.GetComponent<Rigidbody>();
        if (rb) rb.linearVelocity = firePoint.forward * projectileSpeed;
    }
}