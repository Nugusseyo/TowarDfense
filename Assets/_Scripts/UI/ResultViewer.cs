using System;
using _Script.ScriptableObject.Event;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class ResultViewer : MonoBehaviour
    {
        [field: SerializeField] public EventChannelSO GameEventChannelSO { get; private set; }
        
        [field: SerializeField] public GameObject[] HideObjects { get; private set; }
        [field: SerializeField] public GameObject ViewParent { get; private set; }
        [field: SerializeField] public GameObject WinInfo { get; private set; }
        [field: SerializeField] public GameObject LostInfo { get; private set; }

        [SerializeField] private Image backGroundImage;

        [SerializeField] private Color lostBackGroundColor;
        [SerializeField] private Color winBackGroundColor;
        [SerializeField] private Button nextButton;
        
        [SerializeField] private float fadeDuration = 0.5f;

        private CanvasGroup _canvasGroup;
        
        private bool _isActive = false;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            
            GameEventChannelSO.AddListener<GameEndEvent>(HandleResultEvent);
            nextButton.onClick.AddListener(HandleNextButtonClick);
            ViewParent.SetActive(false);
            WinInfo.SetActive(false);
            LostInfo.SetActive(false);
        }
        private void OnDestroy()
        {
            GameEventChannelSO.RemoveListener<GameEndEvent>(HandleResultEvent); //WaveView에서 쓰는중.
            if (nextButton != null)
                nextButton.onClick.RemoveListener(HandleNextButtonClick);
            
            _canvasGroup.DOKill();
        }
        
        private void HandleNextButtonClick()
        {
            if (_isActive) return;
            _isActive = true;           //한번만 야르딱딱 누르게 해줘야지.
            
            ScreenEffectManager.Instance.ScreenFade(true, 0.25f, true);
            Debug.Log("버튼 눌리긴 하네;");
        }


        //IsLost다. 졌으면 True고 아니면 False다. 너는 할 수 있어 야르
        private void HandleResultEvent(GameEndEvent evt)
        {
            ViewParent.SetActive(true);
            foreach (GameObject hide in HideObjects)
            {
                hide.SetActive(false);
            }
            if (evt.IsLost) //이게 졌을때임.
            {
                //backGroundImage.color = Color.red; //이거 변수로 빼놓을까
                //빼놓자 회사가면 이렇게 하겠지
                backGroundImage.color = lostBackGroundColor;
                LostInfo.SetActive(true);
            }
            else
            {
                backGroundImage.color = winBackGroundColor;
                WinInfo.SetActive(true);
            }
            //여기서 페이드 (알파값 0->1) 해줌.
            _canvasGroup.DOKill();
            _canvasGroup.alpha = 0f;
            
            _canvasGroup.DOFade(1f, fadeDuration).SetUpdate(true);
            //아니 이거 코루틴으로 해야하는줄 알았는데 DOTween 되는거였어? 인생

            Time.timeScale = 0;
        }
    }
}
