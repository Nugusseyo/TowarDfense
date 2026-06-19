using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class SettingUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject settingsPanel;      // 설정창 최상위 오브젝트
        [SerializeField] private CanvasGroup bgCanvasGroup;     // 검은색 배경
        [SerializeField] private RectTransform windowTransform; // 설정창 실제 팝업 창

        [Header("Animation Settings")]
        [SerializeField] private float animDuration = 0.3f;     // 애니메이션 속도
        [SerializeField] private float maxBgAlpha = 1f;       // 배경 검은색의 최대 투명도 (0~1)
        
        [Header("Warning Popup")]
        [SerializeField] private GameObject warningPopup;
        [SerializeField] private TextMeshProUGUI warningText;
        [SerializeField] private CanvasGroup warningGroup;
        [SerializeField] private Button yesBtn;
        [SerializeField] private Button noBtn;
        
        [Header("Warning Animation Settings")]
        [SerializeField] private float warningDuration = 0.3f;     // 애니메이션 속도

        public event Action OnButtonYes;
        public event Action OnButtonNo;


        private float _prevTimeScale;

        private void Awake()
        {
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }

            if (yesBtn != null)
            {
                yesBtn.onClick.AddListener(() =>
                {
                    OnButtonYes?.Invoke();
                    OnButtonYes = null;
                    CloseAllPopUp();
                });
            }

            if (noBtn != null)
            {
                noBtn.onClick.AddListener(() =>
                {
                    OnButtonNo?.Invoke();
                    OnButtonNo = null;
                    WarningPopClose();
                });
            }
        }

        private void CloseAllPopUp()
        {
            WarningPopClose();
            CloseSettings();
        }
        public void OpenSettings()
        {
            _prevTimeScale = Time.timeScale;
            Time.timeScale = 0;
            
            bgCanvasGroup.DOKill();
            windowTransform.DOKill();
            
            settingsPanel.SetActive(true);
            bgCanvasGroup.alpha = 0f;
            windowTransform.localScale = Vector3.zero;

            bgCanvasGroup.DOFade(maxBgAlpha, animDuration).SetUpdate(true);

            
            windowTransform.DOScale(Vector3.one, animDuration)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);
        }
        
        public void CloseSettings()
        {
            Time.timeScale = _prevTimeScale;
            
            bgCanvasGroup.DOKill();
            windowTransform.DOKill();
            
            bgCanvasGroup.DOFade(0f, animDuration).SetUpdate(true);
            
            windowTransform.DOScale(Vector3.zero, animDuration)
                .SetEase(Ease.InBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    settingsPanel.SetActive(false);
                });
        }

        public void WarningPopUp(string viewText)
        {
            warningGroup.alpha = 0f;
            warningText.text = viewText;
            warningPopup.SetActive(true);
            
            warningGroup.DOFade(1f, warningDuration).SetUpdate(true);
        }
        private void WarningPopClose()
        {
            warningGroup.DOFade(0f, warningDuration).SetUpdate(true).OnComplete(() =>
            {
                warningGroup.alpha = 0f;
                warningPopup.SetActive(false);
            });
        }
    }
}
