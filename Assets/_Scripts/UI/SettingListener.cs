using System;
using _Scripts.Managers.LifeManager;
using _Scripts.Managers.Scene;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.UI
{
    public class SettingListener : MonoBehaviour
    {
        public UnityEvent OnYesBtnClick;
        [SerializeField] private SettingUI settingUI;
        [SerializeField] private string viewText;

        private SceneChanger _sceneChanger;

        private void Awake()
        {
            _sceneChanger = GetComponent<SceneChanger>();
        }

        public void ViewWarning()
        {
            settingUI.OnButtonYes += () => OnYesBtnClick?.Invoke();
            settingUI.WarningPopUp(viewText);
        }

        public void ZeroLife()
        {
            if(LifeManager.Instance.CurrentLife != 0)
                LifeManager.Instance.CurrentLife = 0;
        }

        public void BackToSelect()
        {
            if(_sceneChanger != null)
                _sceneChanger.Restart();
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
