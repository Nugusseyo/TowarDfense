using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-20)]

public class DebuggingTestClass : MonoBehaviour
{
    private void Awake()
    {
        UnitySettingManager.IsKeepLogging(false);
        Debug.Log("디버깅 1");
        Debug.LogWarning("디버깅 2");
        Debug.LogError("디버깅 3");
    }
}
