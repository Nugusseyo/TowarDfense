using UnityEngine;
using UnityEngine.UI;
using HealthModule = _Script.Agent;

namespace GameModules._Script.Agent.CombatSystem
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthBar;

        private Transform _followTarget;
        private HealthModule.Agent _agent;
        private global::_Script.Agent.Modules.LegacyHealthModule _legacyHealthModule;


        public float NormalizedHealthValue => Mathf.Clamp01(_legacyHealthModule.CurrentHealth / _legacyHealthModule.MaxHealth);

        public void Initialize(HealthModule.Agent agent)
        {
            _agent = agent;

            _followTarget = agent.transform;
            
            _legacyHealthModule = _agent.GetModule<global::_Script.Agent.Modules.LegacyHealthModule>();
            Debug.Assert(_legacyHealthModule != null, $"{gameObject.name}이 Agent의 HealthModule을 찾지 못했습니다!");

            UpdateLegacyHealthBar(0 ,_legacyHealthModule.CurrentHealth, _legacyHealthModule.MaxHealth);
            _legacyHealthModule.OnHealthChanged += UpdateLegacyHealthBar;
        }
        
        private void UpdateLegacyHealthBar(float prevHealth, float currentHealth, float max)
        {
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                return;
            }
            healthBar.fillAmount = currentHealth / max;
        }
    }
}