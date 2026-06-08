using System;
using _Script.ScriptableObject.Event;
using _Script.Tools.Utility;
using _Scripts.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

namespace _Scripts.Managers.InfoM
{
    public class InfoManager : MonoSingleton<InfoManager>
    {
        [field: SerializeField] public InputSO InputSO { get; private set; }
        [field: SerializeField] public EventChannelSO ViewUIEventChannelSO { get; private set; }
        
        [SerializeField] private LayerMask infoTargetLayer;
        
        [SerializeField] private GameObject rangeDecal;
        private DecalProjector _decal;
        
        public Agent.Agent TargetInfo { get; private set; }
        
        private bool _isClickRequested = false;
        private bool _wasDecal = false;

        protected override void Awake()
        {
            base.Awake();
            InputSO.OnInGameClick += HandleInGameClick;

            if (rangeDecal != null)
            {
                rangeDecal.SetActive(false);
                _decal = rangeDecal.GetComponent<DecalProjector>();
                ViewUIEventChannelSO.AddListener<AgentInfoUI>(HideRangeDecal);

                _wasDecal = true;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            InputSO.OnInGameClick -= HandleInGameClick;
            
            if(_wasDecal)
                ViewUIEventChannelSO.AddListener<AgentInfoUI>(HideRangeDecal);
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
                TargetInfo = null;
                ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentOnUI.Init(null));
                HideRangeDecal();
                return;
            }
            
            if (target.TryGetComponent<Agent.Agent>(out Agent.Agent info))
                TargetInfo = info;
            else
                TargetInfo = null;
            
            if (TargetInfo == null) 
            {
                ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentOnUI.Init(null));
                HideRangeDecal();
                return;
            }
            
            ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentOnUI.Init(TargetInfo));
            ShowRangeDecal(TargetInfo);
        }
        
        private void ShowRangeDecal(Agent.Agent agent)
        {
            if (rangeDecal == null) return;

            Vector3 decalPos = agent.transform.position;
            decalPos.y += 0.2f;
            rangeDecal.transform.position = decalPos;
            
            float range = agent.AgentStatusSO.DetectRadius;
            float size = range * 2f;
            
            _decal.size = new Vector3(size, size, size);
            rangeDecal.SetActive(true);
        }
        
        public void HideRangeDecal()
        {
            if (rangeDecal != null)
            {
                rangeDecal.SetActive(false);
            }
        }
        private void HideRangeDecal(AgentInfoUI evt)
        {
            if (rangeDecal != null)
            {
                rangeDecal.SetActive(false);
            }
        }

        public void AgentDeathLogic(Agent.Agent deathTarget)
        {
            if (deathTarget != TargetInfo) return;

            HideRangeDecal();
            ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentOnUI.Init(null));
        }
    }
}