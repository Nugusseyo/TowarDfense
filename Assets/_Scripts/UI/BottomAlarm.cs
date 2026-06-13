using System;
using _Script.ScriptableObject.Event;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class BottomAlarm : MonoBehaviour
    {
        [field: SerializeField] public EventChannelSO UIEventChannel { get; private set; }

        [SerializeField] private TextMeshProUGUI warningText;
        
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private Sequence _warningSequence;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            
            InitializeState();
            UIEventChannel.AddListener<AlarmUI>(HandleShowAlarm);
        }

        private void OnDestroy()
        {
            UIEventChannel.RemoveListener<AlarmUI>(HandleShowAlarm);
        }

        private void HandleShowAlarm(AlarmUI evt)
        {
            ShowWarning(evt.AlarmText);
        }

        private void InitializeState()
        {
            _warningSequence?.Kill();
            
            _canvasGroup.alpha = 0f;
            _rectTransform.anchoredPosition = new Vector2(0f, -150f);
        }
        
        public void ShowWarning(string message)
        {
            InitializeState();
            
            if (warningText != null) warningText.text = message;
            
            _warningSequence = DOTween.Sequence();
            
            _warningSequence.Join(_canvasGroup.DOFade(1f, 0.3f));
            _warningSequence.Join(_rectTransform.DOAnchorPosY(50f, 0.4f).SetEase(Ease.OutBack));
            
            _warningSequence.AppendInterval(1.5f);
            _warningSequence.Append(_canvasGroup.DOFade(0f, 0.5f))
                             .OnComplete(InitializeState);
        }
    }
}
