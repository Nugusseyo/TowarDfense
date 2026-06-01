using System;
using _Script.ScriptableObject.Event;
using _Script.Tools.Utility;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Managers.InfoM
{
    public class InfoManager : MonoSingleton<InfoManager>
    {
        [field: SerializeField] public InputSO InputSO { get; private set; }
        [field: SerializeField] public EventChannelSO ViewUIEventChannelSO { get; private set; }
        
        [SerializeField] private LayerMask infoTargetLayer;
        
        public Agent.Agent TargetInfo { get; private set; }

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
            if (!InputSO.IsInGame) return;
            Debug.Log("야루");
            GameObject target = InputSO.GetGameObject(infoTargetLayer);
            
            if (target == null) //아무것도 안집힘. 그럼 UI 꺼줘야해
            {
                //UI OFF
                ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentOnUI.Init(null));
                return;
            }
            
            if (target.TryGetComponent<Agent.Agent>(out Agent.Agent info))
                TargetInfo = info;
            else
                TargetInfo = null;
            
            if (TargetInfo == null) //아무것도 안집힘. 그럼 UI 꺼줘야해
            {
                //UI OFF
                ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentOnUI.Init(null));
                return;
            }
            
            ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentOnUI.Init(TargetInfo));
        }
    }
}
