using System;
using _Script.Agent.Modules;
using _Scripts.Agent;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class BarInfoUI : MonoBehaviour, IModule
    {
        private ModuleAgent _moduleAgent;
        private Transform _cameraTransform;

        [SerializeField] private Image healthBar;
        [SerializeField] private Image skillBar;
        
        private HealthModule _healthModule;
        private IAgentSkillModule _skillModule;
        public void Initialize(ModuleAgent moduleAgent)
        {
            _moduleAgent = moduleAgent;
        }

        private void Start()
        {
            _healthModule = _moduleAgent.GetModule<HealthModule>();
            _skillModule = _moduleAgent.GetModule<IAgentSkillModule>();
            _cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            if(healthBar != null && _healthModule != null)
                healthBar.fillAmount = _healthModule.GetHealthNormal;
            
            if(skillBar != null && _skillModule != null)
                skillBar.fillAmount = _skillModule.GetCooldownNormal;
        }

        private void LateUpdate()
        {
            if (_cameraTransform == null)
            {
                if (Camera.main != null) 
                    _cameraTransform = Camera.main.transform;
                return;
            }
            transform.rotation = _cameraTransform.rotation;
        }
    }
}
