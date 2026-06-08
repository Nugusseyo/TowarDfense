using System;
using _Script.ScriptableObject.Event;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class SelectorViewer : MonoBehaviour
    {
        [field: SerializeField] public EventChannelSO ButtonEventChannel { get; private set; }
        [SerializeField] private GameObject uiParent;
        [SerializeField] private TextMeshProUGUI stageText;
        [SerializeField] private Image map;
        [SerializeField] private SceneChanger sceneChanger;

        private void Awake()
        {
            ButtonEventChannel.AddListener<ButtonUI>(HandleViewUI);
        }

        private void HandleViewUI(ButtonUI evt)
        {
            if (evt.Button == null)
            {
                if(EventSystem.current != null && !EventSystem.current.IsPointerOverGameObject())
                    uiParent.SetActive(false);
            }
            else
            {
                uiParent.SetActive(true);
                stageText.text = evt.Button.stage;
                map.sprite = evt.Button.map;
                sceneChanger.index = evt.Button.index;
            }
        }
    }
}
