using System;
using _Script.ScriptableObject.Event;
using _Script.Tools.Utility;
using UnityEngine;

namespace _Scripts.Managers.CostManager
{
    public class CostManager : MonoSingleton<CostManager>
    {
        public int startCost;
        public int maxCost;
        public float costPerSecond;

        private float _costTimer = 0;

        private int _cost;
        public int Cost
        {
            get => _cost;
            set => _cost = Mathf.Clamp(value, 0, maxCost);
        }

        [field: SerializeField] public EventChannelSO GameManagerEventChannel { get; private set; }

        public float GetNormalCost => Mathf.Clamp01(_costTimer / costPerSecond);

        private bool _isRunning = false;
        private void Awake()
        {
            Cost = startCost;
        }
        
        private void Update()
        {
            if (!_isRunning || _cost == maxCost)
            {
                _costTimer = 0;
                return;
            }
            
            _costTimer += Time.deltaTime;

            if (_costTimer >= costPerSecond)
            {
                _costTimer -= costPerSecond;
                Cost++;
            }
        }
    }
}
