using UnityEngine;

[DisallowMultipleComponent]
public class EnemyObject : MonoBehaviour
{
    public EnemyRegistrySO registry;
    public Transform target;
    public float moveDelay = 5f;
    public float moveSpeed = 3f;
    private float timer;
    private bool isMoving;

    private void OnEnable()
    {
        if (registry != null) 
            registry.Register(this);
    }

    private void OnDisable()
    {
        if (registry != null) 
            registry.Unregister(this);
    }

    private void Start()
    {
        timer = 0f;
        isMoving = false;
    }

    private void Update()
    {
        if (!target)
            return;

        if (!isMoving)
        {
            timer += Time.deltaTime;
            if (timer >= moveDelay) isMoving = true;
        }
        else
        {
            var direction = (target.position - transform.position).normalized;
            transform.position += direction * (moveSpeed * Time.deltaTime);
        }
    }
}