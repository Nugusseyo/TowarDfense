using UnityEngine;

public class BulletController : MonoBehaviour
{
    public ElementData elementData;
    
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
