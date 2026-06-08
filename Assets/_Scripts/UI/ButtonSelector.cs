using System;
using _Script.ScriptableObject.Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.UI
{
    public class ButtonSelector : MonoBehaviour
    {
        [field: SerializeField] public InputSO InputSO;
        [SerializeField] private LayerMask buttonLayer;

        [field: SerializeField] public EventChannelSO ButtonEventChannel { get; private set; }

        private void Awake()
        {
            InputSO.ChangeInput(false);
            InputSO.OnLeftBtnClickEnd += HandleButtonClick;
        }

        private void OnDestroy()
        {
            InputSO.OnLeftBtnClickEnd -= HandleButtonClick;
        }

        private void HandleButtonClick()
        {
            GameObject target = InputSO.GetGameObject(buttonLayer);
            if (target == null)
            {
                ButtonEventChannel.RaiseEvent(UIEvents.ButtonUI.Init(null));
                return;
            }

            if (!target.TryGetComponent(out ObjectButton button))
            {
                ButtonEventChannel.RaiseEvent(UIEvents.ButtonUI.Init(null));
                return;
            }

            ButtonEventChannel.RaiseEvent(UIEvents.ButtonUI.Init(button));
        }
    }
}
