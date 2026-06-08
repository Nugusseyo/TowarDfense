using System.Collections;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public Transform firePoint;
    public Renderer topRenderer;
    public TankData tankData;

    private void Start()
    {
        topRenderer.material = tankData.tankMaterial;
        StartCoroutine(FireProjectileAfterDelay(1f));
    }

    private IEnumerator FireProjectileAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        var projectile = Instantiate(tankData.projectilePrefab, firePoint.position, firePoint.rotation);
        
        var rb = projectile.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.AddForce(firePoint.forward * tankData.firePower, ForceMode.Impulse);
        }
    }
}