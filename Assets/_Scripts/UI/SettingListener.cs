using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.UI
{
    public class SettingListener : MonoBehaviour
    {
        public UnityEvent OnYesBtnClick;
        [SerializeField] private SettingUI settingUI;
        [SerializeField] private string viewText;

        public void ViewWarning()
        {
            settingUI.OnButtonYes += () => OnYesBtnClick?.Invoke();
            settingUI.WarningPopUp(viewText);
        }
    }
}
