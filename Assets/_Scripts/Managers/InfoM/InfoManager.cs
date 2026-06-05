using System;
using _Script.ScriptableObject.Event;
using _Script.Tools.Utility;
using _Scripts.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Managers.InfoM
{
    public class InfoManager : MonoSingleton<InfoManager>
    {
        [field: SerializeField] public InputSO InputSO { get; private set; }
        [field: SerializeField] public EventChannelSO ViewUIEventChannelSO { get; private set; }
        
        [SerializeField] private LayerMask infoTargetLayer;
        
        public Agent.Agent TargetInfo { get; private set; }
        
        private bool _isClickRequested = false;

        protected override void Awake()
        {
            base.Awake();
            InputSO.OnInGameClick += HandleInGameClick;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            InputSO.OnInGameClick -= HandleInGameClick;
        }

        
        private void HandleInGameClick()
        {
            _isClickRequested = true; //자꾸 EventSystem이랑 충돌나서 이렇게 처리해야됨;;
        }

        
        private void Update()
        {
            //EventSystem이랑 New InputSystem이랑 타이밍이 안맞아서, 자꾸 경고가 잔뜩 뜸.
            //그래서 New InputSystem을 bool값으로 제어해서 연산이 끝난 뒤 프레임에서 함수를 실행시켜주기로 했음.
            if (_isClickRequested)
            {
                _isClickRequested = false; 

                if (!InputSO.IsInGame) return;
                
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) 
                {
                    return;
                }
                
                InGameClickLogic();
            }
        }
        
        private void InGameClickLogic()
        {
            Debug.Log("야루");
            GameObject target = InputSO.GetGameObject(infoTargetLayer);
            
            if (target == null) 
            {
                ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentOnUI.Init(null));
                return;
            }
            
            if (target.TryGetComponent<Agent.Agent>(out Agent.Agent info))
                TargetInfo = info;
            else
                TargetInfo = null;
            
            if (TargetInfo == null) 
            {
                ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentOnUI.Init(null));
                return;
            }
            
            ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentOnUI.Init(TargetInfo));
        }
    }
}