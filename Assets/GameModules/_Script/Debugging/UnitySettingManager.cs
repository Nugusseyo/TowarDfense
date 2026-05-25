using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitySettingManager
{
    /// <summary>
    /// beLogging이 true이면 로그를 계속 출력하고, false이면 로그 출력을 중지합니다.
    /// </summary>
    /// <param name="beLogging">이 변수가 True라면 디버깅을 합니다.</param>
    public static void IsKeepLogging(bool beLogging)
    {
        Debug.unityLogger.logEnabled = beLogging;
    }
}
