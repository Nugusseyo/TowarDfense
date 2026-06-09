using System;
using _Script.ScriptableObject.Event;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.UI
{
    public class SwapViewer : MonoBehaviour
    {
        [field: SerializeField] public EventChannelSO GameEventChannel { get; private set; }
        
        [SerializeField] private RectTransform topBar;
        [SerializeField] private RectTransform bottomBar;
        
        [SerializeField] private float duration = 2.5f;
        private void Awake()
        {
            GameEventChannel.AddListener<GameSwapEvent>(FadeScreen);
        }

        private void OnDestroy()
        {
            GameEventChannel.RemoveListener<GameSwapEvent>(FadeScreen);
        }

        private void FadeScreen(GameSwapEvent evt)
        {
            if(!evt.FadeIn)
                Time.timeScale = 0f;
        
            topBar.DOSizeDelta(new Vector2(topBar.sizeDelta.x, evt.Height), duration)
                .SetEase(Ease.InOutCubic).SetUpdate(true);

            bottomBar.DOSizeDelta(new Vector2(bottomBar.sizeDelta.x, evt.Height), duration)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() =>
                {
                    if(evt.FadeIn)
                        Time.timeScale = 1f;
                }).SetUpdate(true);
        }
    }
}
