using System.Collections; // 💡 코루틴을 위해 추가
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace _Scripts.Managers.Scene
{
    public class SelectSceneLoader : MonoBehaviour
    {
        [SerializeField] private Volume globalVolume;
        [SerializeField] private CanvasGroup blackScreenCanvasGroup;
        [SerializeField] private float effectDuration = 0.8f;
        
        [SerializeField] private bool playIntroOnStart = true;

        private void Start()
        {
            if (playIntroOnStart)
            {
                // 💡 첫 프레임 렉 스파이크를 피하기 위해 코루틴으로 실행합니다.
                StartCoroutine(PlayTransitionRoutine(true));
            }
        }

        // 💡 외부에서 강제로 부를 때를 대비해 기존 일반 함수 형태도 유지
        public void PlayTransition(bool isFadeIn)
        {
            StartCoroutine(PlayTransitionRoutine(isFadeIn));
        }

        private IEnumerator PlayTransitionRoutine(bool isFadeIn)
        {
            // 💡 [핵심] 씬이 시작되자마자 발생하는 로딩 렉(Hitch) 프레임이 
            // 완전히 지나갈 때까지 딱 1프레임 동안 숨을 고르고 대기합니다.
            yield return null; 

            if (globalVolume == null && blackScreenCanvasGroup == null)
            {
                Time.timeScale = 1f;
                yield break;
            }

            float targetAlpha = isFadeIn ? 0f : 1f;
            float targetWeight = isFadeIn ? 0f : 1f;
            float targetTimeScale = isFadeIn ? 1f : 0f;
            
            if (isFadeIn)
            {
                Time.timeScale = 0f;
                if (globalVolume != null) globalVolume.weight = 1f;
                if (blackScreenCanvasGroup != null)
                {
                    blackScreenCanvasGroup.gameObject.SetActive(true);
                    blackScreenCanvasGroup.alpha = 1f;
                }
            }
            else
            {
                if (blackScreenCanvasGroup != null)
                {
                    blackScreenCanvasGroup.gameObject.SetActive(true);
                    blackScreenCanvasGroup.alpha = 0f;
                }
            }

            Sequence transitionSequence = DOTween.Sequence();

            if (blackScreenCanvasGroup != null)
            {
                Ease alphaEase = isFadeIn ? Ease.OutCubic : Ease.InCubic;
                // 💡 개별 트윈 뒤에도 .SetUpdate(true)를 붙여 타임스케일 0 상태에서의 구동을 완벽히 보장합니다.
                transitionSequence.Join(blackScreenCanvasGroup.DOFade(targetAlpha, effectDuration)
                    .SetEase(alphaEase).SetUpdate(true));
            }

            if (globalVolume != null)
            {
                Ease weightEase = isFadeIn ? Ease.OutQuad : Ease.InQuad;
                float duration = isFadeIn ? effectDuration + 0.4f : effectDuration;
                
                transitionSequence.Join(DOTween.To(() => globalVolume.weight, x => globalVolume.weight = x, targetWeight, duration)
                    .SetEase(weightEase).SetUpdate(true));
            }

            Ease timeEase = isFadeIn ? Ease.InQuart : Ease.OutQuart;
            transitionSequence.Join(DOTween.To(() => Time.timeScale, x => Time.timeScale = x, targetTimeScale, effectDuration)
                .SetEase(timeEase).SetUpdate(true)); 
            
            transitionSequence.SetUpdate(true);

            transitionSequence.OnComplete(() =>
            {
                Time.timeScale = targetTimeScale;
                if (globalVolume != null) globalVolume.weight = targetWeight;
            
                if (blackScreenCanvasGroup != null)
                {
                    blackScreenCanvasGroup.alpha = targetAlpha;
                    if (isFadeIn)
                    {
                        blackScreenCanvasGroup.gameObject.SetActive(false); 
                    }
                }
            });
        }
    }
}