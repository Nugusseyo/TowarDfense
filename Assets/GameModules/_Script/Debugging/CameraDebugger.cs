using System;
using _Script.Camera;
using UnityEngine;

public class CameraDebugger : MonoBehaviour
{
    private Transform _cameraTransform;

    private void Awake()
    {
        _cameraTransform = GameObject.Find("CameraBundle").transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("J눌림");
            _cameraTransform.rotation = Quaternion.Euler(70, 20, 20);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("K눌림");
            _cameraTransform.rotation = Quaternion.Euler(70, 0, 0);
        }
    }
}
