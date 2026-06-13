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
        
        private Agent.Combat.HealthModule _healthModule;
        private IAgentSkillModule _skillModule;
        public void Initialize(ModuleAgent moduleAgent)
        {
            _moduleAgent = moduleAgent;
            if(_moduleAgent == null) Debug.Log("왜 안되냐고");
        }

        private Transform _followTarget;
        private Vector3 _offset;

        private void Start()
        {
            _healthModule = _moduleAgent.GetModule<Agent.Combat.HealthModule>();
            Debug.Assert(_healthModule != null, $"HealthModule이 Null이네요;;");
            _skillModule = _moduleAgent.GetModule<IAgentSkillModule>();
            _cameraTransform = Camera.main.transform;
            
            if (transform.parent != null)
            {
                _followTarget = transform.parent;
                _offset = transform.localPosition;
                transform.SetParent(null);
            }

            _healthModule.OnDeath.AddListener(HandleDeath);
        }

        private void HandleDeath()
        {
            if(_healthModule != null)
                _healthModule.OnDeath.RemoveListener(HandleDeath);

            Destroy(gameObject);
        }

        private void Update()
        {
            if (_followTarget != null)
            {
                transform.position = _followTarget.position + _offset;
            }
            
            if (healthBar != null && _healthModule != null && !float.IsNaN(_healthModule.GetHealthNormal))
            {
                healthBar.fillAmount = _healthModule.GetHealthNormal;
            }
    
            if(skillBar != null && _skillModule != null && !float.IsNaN(_skillModule.GetCooldownNormal))
                skillBar.fillAmount = _skillModule.GetCooldownNormal;
        }

        private void LateUpdate()
        {
            if (_cameraTransform == null) return;
            
            transform.rotation = _cameraTransform.rotation;
        }
    }
}
