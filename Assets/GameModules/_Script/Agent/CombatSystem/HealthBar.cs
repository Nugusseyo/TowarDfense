using System;
using _Script.Agent.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace _Script.Agent.CombatSystem
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        
        private Agent _agent;
        private HealthModule _healthModule;


        public float NormalizedHealthValue => Mathf.Clamp01(_healthModule.CurrentHealth / _healthModule.MaxHealth);

        private void Awake()
        {
            _agent = GetComponentInParent<Agent>();
        }

        private void Start()
        {
            _healthModule = _agent.GetModule<HealthModule>();
            Debug.Assert(_healthModule != null, $"{gameObject.name}이 Agent의 HealthModule을 찾지 못했습니다!");

            UpdateHealthBar(0 ,_healthModule.CurrentHealth, _healthModule.MaxHealth);
            _healthModule.OnHealthChanged += UpdateHealthBar;
        }

        private void UpdateHealthBar(float prevHealth, float currentHealth, float max)
        {
            healthBar.fillAmount = currentHealth / max;
        }
    }
}