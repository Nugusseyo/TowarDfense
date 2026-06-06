using DG.Tweening;
using UnityEngine;

namespace _Scripts.UI
{
    public class StampUI : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            _rectTransform.DOKill();
            _canvasGroup.DOKill();
            
            _rectTransform.localScale = Vector3.one * 4f; 
            _rectTransform.localRotation = Quaternion.Euler(0, 0, -20f);
            _canvasGroup.alpha = 0f;

            Sequence stampSeq = DOTween.Sequence();

            stampSeq.SetUpdate(true);
            stampSeq.AppendInterval(2.5f);
            
            stampSeq.Join(_canvasGroup.DOFade(1f, 0.1f));
            stampSeq.Join(_rectTransform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack, 2.5f));
            stampSeq.Join(_rectTransform.DOLocalRotate(new Vector3(0, 0, 30f), 0.3f).SetEase(Ease.OutBack, 2.0f));
        }

        private void OnDisable()
        {
            _rectTransform.DOKill();
            _canvasGroup.DOKill();
        }
    }
}
