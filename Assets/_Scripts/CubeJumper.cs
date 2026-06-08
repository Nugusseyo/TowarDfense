using UnityEngine;

public class CubeJumper : MonoBehaviour
{
    public JumpData jumpData;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError("Rigidbody를 찾을 수 없습니다.");
            return;
        }

        InvokeRepeating(nameof(Jump), 0f, 3f);
    }


    private void Jump()
    {
        if (jumpData && _rb)
            _rb.AddForce(Vector3.up * jumpData.jumpPower, ForceMode.Impulse);
    }
}