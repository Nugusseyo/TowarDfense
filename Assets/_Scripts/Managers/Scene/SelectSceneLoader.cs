using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace _Scripts.Managers.Scene
{
    public class SelectSceneLoader : MonoBehaviour
    {
        [Header("Visuals")]
        [SerializeField] private Volume globalVolume;
        [SerializeField] private CanvasGroup blackScreenCanvasGroup;
        [SerializeField] private float effectDuration = 0.8f;
        
        [Header("Audio")]
        [SerializeField] private AudioSource bgmSource; 
        [SerializeField] private float maxBgmVolume = 1f;

        [Header("Settings")]
        [SerializeField] private bool playIntroOnStart = true;
        [SerializeField] private GraphicRaycaster graphicRaycaster;

        private void Start()
        {
            if (playIntroOnStart)
            {
                StartCoroutine(PlayTransitionRoutine(true));
            }
        }
        
        public void PlayTransition(bool isFadeIn)
        {
            StartCoroutine(PlayTransitionRoutine(isFadeIn));
        }

        private IEnumerator PlayTransitionRoutine(bool isFadeIn)
        {
            yield return null;
            if (graphicRaycaster != null)
                graphicRaycaster.enabled = true;

            if (globalVolume == null && blackScreenCanvasGroup == null)
            {
                Time.timeScale = 1f;
                yield break;
            }

            float targetAlpha = isFadeIn ? 0f : 1f;
            float targetWeight = isFadeIn ? 0f : 1f;
            float targetTimeScale = isFadeIn ? 1f : 0f;
            
            float targetVolume = isFadeIn ? maxBgmVolume : 0f;
            
            if (isFadeIn)
            {
                Time.timeScale = 0f;
                if (globalVolume != null) globalVolume.weight = 1f;
                if (blackScreenCanvasGroup != null)
                {
                    blackScreenCanvasGroup.gameObject.SetActive(true);
                    blackScreenCanvasGroup.alpha = 1f;
                }
                
                if (bgmSource != null) bgmSource.volume = 0f; 
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

            if (bgmSource != null)
            {
                transitionSequence.Join(bgmSource.DOFade(targetVolume, effectDuration)
                    .SetEase(Ease.InOutQuad)
                    .SetUpdate(true));
            }

            if (blackScreenCanvasGroup != null)
            {
                Ease alphaEase = isFadeIn ? Ease.OutCubic : Ease.InCubic;
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
                if (!isFadeIn && bgmSource != null) bgmSource.Stop();
                
                if (graphicRaycaster != null)
                    graphicRaycaster.enabled = false;
            });
        }
    }
}