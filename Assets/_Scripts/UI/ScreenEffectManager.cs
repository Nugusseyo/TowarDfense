using System;
using System.Collections;
using _Script.ScriptableObject.Event;
using _Script.Tools.Utility;
using UnityEngine;

namespace _Scripts.UI
{
    public class ScreenEffectManager : MonoSingleton<ScreenEffectManager>
    {
        [field: SerializeField] public EventChannelSO GameEventChannel { get; private set; }

        private Coroutine _fadeCoroutine;
        private float _prevTimeScale = 1f;

        private void Start()
        {
            ScreenFade(false, 4f, true);
        }

        public void ScreenFade(bool isFadeIn, float duration, bool isFreeze)
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }

            if (isFreeze)
            {
                _prevTimeScale = Time.timeScale;
                
                Time.timeScale = 0f;
            }
            
            _fadeCoroutine = StartCoroutine(FadeValue(isFadeIn, duration, isFreeze));
        }

        private IEnumerator FadeValue(bool isFadeIn, float duration, bool isFreeze)
        {
            float curTime = 0;
            
            float startValue = isFadeIn ? 0f : 1f;
            float endValue = isFadeIn ? 1f : 0f;

            while (curTime < duration)
            {
                curTime += Time.unscaledDeltaTime;
                //시간의 영향을 받지 않고 ㄹㅇ 현실 시간 기준으로 deltaTime반환.
                //deltaTime은 Scale 변하면 값이 변하지만, 얘는 영원히 deltaTime을 반환해줌.
                
                float t = Mathf.Clamp01(curTime / duration);
                float normal = Mathf.Lerp(startValue, endValue, t);
                
                GameEventChannel.RaiseEvent(InGameEvents.GameSwapEvent.Init(normal));
                yield return null;
            }
            
            GameEventChannel.RaiseEvent(InGameEvents.GameSwapEvent.Init(endValue));
            
            if (isFreeze)
            {
                Time.timeScale = _prevTimeScale;
            }
            
            _fadeCoroutine = null;
        }
    }
}