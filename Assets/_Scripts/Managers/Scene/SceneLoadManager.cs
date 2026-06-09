using System;
using _Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Managers.Scene
{
    [DefaultExecutionOrder(-100)]
    public class SceneLoadManager : MonoBehaviour
    {
        private void OnEnable()
        {
            SceneManager.sceneLoaded += HandleSceneLoad;
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= HandleSceneLoad;
        }

        private void HandleSceneLoad(UnityEngine.SceneManagement.Scene arg0, LoadSceneMode arg1)
        {
            Debug.Log("이거 진짜 됨?");
            ScreenEffectManager.Instance.GameStartFade();
        }
    }
}
