using System;
using _Script.ScriptableObject.Event;
using _Scripts.Agent.Tower;
using _Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace _Scripts.Managers.InfoM
{
    public class InfoHealthViewer : MonoBehaviour
    {
        [field: SerializeField] public EventChannelSO ViewUIEventChannel { get; private set; }
        [SerializeField] private TextMeshProUGUI healthText;
        private Image _healthImage;

        private Agent.Agent _holdAgent;
        

        private void Awake()
        {
            _healthImage = GetComponent<Image>();
            ViewUIEventChannel.AddListener<AgentOnUI>(HandleOperatorHealth);
            ViewUIEventChannel.AddListener<AgentInfoUI>(HandleViewHealth);
        }

        private void OnDestroy()
        {
            ViewUIEventChannel.RemoveListener<AgentOnUI>(HandleOperatorHealth);
            ViewUIEventChannel.RemoveListener<AgentInfoUI>(HandleViewHealth);
        }

        private void HandleOperatorHealth(AgentOnUI evt)
        {
            if (evt.NextAgent == null || evt.NextAgent.HealthModule == null)
            {
                return;
            }
            
            _holdAgent = evt.NextAgent;
        }

        private void Update()
        {
            if (_holdAgent == null || _holdAgent.HealthModule == null) return;

            _healthImage.fillAmount = _holdAgent.HealthModule.GetHealthNormal;
            healthText.text = _holdAgent.HealthModule.Health + "/" + _holdAgent.HealthModule.MaxHealth;
        }

        private void HandleViewHealth(AgentInfoUI evt)
        {
            if (evt.Agent == null || evt.Agent.UIData == null)
            {
                return;
            }
            _holdAgent = evt.Agent;
            
            _healthImage.fillAmount = 1;
            healthText.text = evt.Agent.UIData.health + "/" + evt.Agent.UIData.health;
        }
    }
}
