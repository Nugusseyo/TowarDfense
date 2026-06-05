using System;
using _Script.ScriptableObject.Event;
using UnityEngine;

namespace _Scripts.UI
{
    public class SwapViewer : MonoBehaviour
    {
        [field: SerializeField] public EventChannelSO GameEventChannel { get; private set; }
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            Debug.Assert(_canvasGroup != null, $"캔버스 그룹이 필수적입니다!");
            
            GameEventChannel.AddListener<GameSwapEvent>(FadeScreen);
        }

        private void OnDestroy()
        {
            GameEventChannel.RemoveListener<GameSwapEvent>(FadeScreen);
        }

        private void FadeScreen(GameSwapEvent evt)
        {
            _canvasGroup.alpha = evt.NormalFade;
        }
    }
}
