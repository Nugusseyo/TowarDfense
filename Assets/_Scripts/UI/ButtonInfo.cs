using _Script.Tools.Utility;
using _Scripts.Managers.InfoM;
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
        
        [SerializeField] private TextMeshProUGUI subTitle;
        [SerializeField] private TextMeshProUGUI numberTitle;
        
        [Header("Three Text!")]
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private TextMeshProUGUI enemyText;
        [SerializeField] private TextMeshProUGUI lifeText;
        
        private MapData _prevData;

        protected override void Awake()
        {
            base.Awake();
            uiParent.SetActive(false);
        }

        public void ButtonViewer(MapData mapData)
        {
            if (_prevData == mapData) return;
            
            canvasGroup.DOKill();
            
            if (mapData == null || (string.IsNullOrEmpty(mapData.Stage) && mapData.Map == null && mapData.Index == 0))
            {
                _prevData = null; 
                
                canvasGroup.DOFade(0f, duration)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() => uiParent.SetActive(false));
                return;
            }
            
            _prevData = mapData;
            
            stageText.text = mapData.Stage;
            sceneChanger.index = mapData.Index;
            mapImage.sprite = mapData.Map;
            
            subTitle.text = mapData.SubTitle;
            waveText.text = "웨이브 : " + mapData.WaveCount;
            lifeText.text = "목숨 : " + mapData.LifeCount;
            enemyText.text = "시민 : " + mapData.EnemyCount + "명";
            numberTitle.text = $"Stage {mapData.BigStageNumber}-{mapData.SmallStageNumber}";

            uiParent.SetActive(true);
            canvasGroup.DOFade(1f, duration).SetEase(Ease.InOutQuad);
        }
    }
}