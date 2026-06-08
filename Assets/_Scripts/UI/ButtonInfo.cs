using _Script.Tools.Utility;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class ButtonInfo : MonoSingleton<ButtonInfo>
    {
        [SerializeField] private GameObject uiParent;
        [SerializeField] private TextMeshProUGUI stageText;
        [SerializeField] private SceneChanger sceneChanger;
        [SerializeField] private Image mapImage;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float duration = 0.5f;

        protected override void Awake()
        {
            base.Awake();
            uiParent.SetActive(false);
        }

        public void ButtonViewer(ButtonListener listener)
        {
            if (string.IsNullOrEmpty(listener.stage) && listener.map == null && listener.index == 0)
            {

                canvasGroup.DOFade(0f, 0.5f).SetEase(Ease.InOutQuad).OnComplete(() => uiParent.SetActive(false));
                return;
            }

            canvasGroup.alpha = 0f;
            canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.InOutQuad);
            
            
            uiParent.SetActive(true);
            stageText.text = listener.stage;
            sceneChanger.index = listener.index;
            mapImage.sprite = listener.map;
        }
    }
}
