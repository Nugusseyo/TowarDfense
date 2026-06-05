using System;
using _Script.ScriptableObject.Event;
using _Scripts.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Scripts.Managers.LifeManager
{
    public class LifeViewer : MonoBehaviour
    {
        
        [field: SerializeField] public EventChannelSO GameEventChannel;
        [SerializeField] private TextMeshProUGUI lifeText;
        private Vector2 _originPos;
        private void Start()
        {
            LifeManager.Instance.OnLifeChanged += HandleLifeChanged;
            lifeText.text = LifeManager.Instance.CurrentLife.ToString();
            _originPos = lifeText.rectTransform.anchoredPosition;
        }

        private void OnDestroy()
        {
            if(LifeManager.Instance != null && LifeManager.Instance.OnLifeChanged != null)
                LifeManager.Instance.OnLifeChanged -= HandleLifeChanged;
            if (lifeText != null)
                lifeText.DOKill();
        }

        private void HandleLifeChanged(int life)
        {
            lifeText.DOKill();
            lifeText.rectTransform.anchoredPosition = _originPos;

            lifeText.rectTransform.DOShakePosition(0.2f, 20f, 20, 90f);
            
            lifeText.text = life.ToString();
            
            if(life <= 0)
                GameEventChannel.RaiseEvent(InGameEvents.GameEndEvent.Init(true));
        }
    }
}
