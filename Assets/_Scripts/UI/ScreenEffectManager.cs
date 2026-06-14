using System;
using System.Collections;
using _Script.ScriptableObject.Event;
using _Script.Tools.Utility;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace _Scripts.UI
{
    public class ScreenEffectManager : MonoSingleton<ScreenEffectManager>
    {
        [field: SerializeField] public EventChannelSO GameEventChannel { get; private set; }

        [SerializeField] private float leaveBar = 150f;
        
        [Header("UI 및 포스트 프로세싱")]
        [SerializeField] private RectTransform gameStartText;       // 위치 초기화용 (중앙 세팅)
        [SerializeField] private CanvasGroup gameStartCanvasGroup; // 💡 글자를 서서히 사라지게 만들기 위한 컴포넌트!
        [SerializeField] private Volume globalVolume;

        protected override void Awake()
        {
            base.Awake();
            GetComponent<CanvasGroup>().alpha = 1;
        }

        public void ScreenFade(bool isFadeIn, float height)
        {
            GameEventChannel.RaiseEvent(InGameEvents.GameSwapEvent.Init(isFadeIn, height));
        }

        public void GameStartFade()
        {
            StartCoroutine(GameStartRoutine());
        }

        private IEnumerator GameStartRoutine()
        {
            //Init
            if (gameStartText != null) gameStartText.anchoredPosition = Vector2.zero;
            if (gameStartCanvasGroup != null) gameStartCanvasGroup.alpha = 1f;

            //위아래 시네마틱 바 닫기
            GameEventChannel.RaiseEvent(InGameEvents.GameSwapEvent.Init(false, leaveBar));
            yield return new WaitForSecondsRealtime(1.0f); 
            
            if (globalVolume != null)
            {
                DOTween.To(() => globalVolume.weight, x => globalVolume.weight = x, 1f, 1f)
                       .SetUpdate(true);
            }
            
            yield return new WaitForSecondsRealtime(1.0f);
            
            if (gameStartCanvasGroup != null)
            {
                gameStartCanvasGroup.DOFade(0f, 1.5f)
                    .SetUpdate(true);
            }
            if (globalVolume != null)
            {
                DOTween.To(() => globalVolume.weight, x => globalVolume.weight = x, 0f, 1.5f)
                    .SetUpdate(true);
            }
            
            GameEventChannel.RaiseEvent(InGameEvents.GameSwapEvent.Init(true, 0));
        }
    }
}