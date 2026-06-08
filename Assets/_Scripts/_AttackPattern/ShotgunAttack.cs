using UnityEngine;

[CreateAssetMenu(fileName = "ShotgunAttack", menuName = "Scriptable Objects/ShotgunAttack")]
public class ShotgunAttack : AttackPattern
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 8f;
    public int pelletCount = 5;
    public float spreadAngle = 15f;

    public override void ExecuteAttack(Transform firePoint)
    {
        for (var i = 0; i < pelletCount; i++)
        {
            var angle = Random.Range(-spreadAngle, spreadAngle);
            var rotation = firePoint.rotation * Quaternion.Euler(0, angle, 0);
            var projectile = Instantiate(projectilePrefab, firePoint.position, rotation);
            
            var rb = projectile.GetComponent<Rigidbody>();
            if (rb) rb.linearVelocity = rotation * Vector3.forward * projectileSpeed;
        }
    }
}