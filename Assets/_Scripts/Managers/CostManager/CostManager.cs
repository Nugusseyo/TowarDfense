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
        
        public float GetNormalCost
        {
            get
            {
                if (Cost >= maxCost) return 1f;
                
                float interval = 1f / costPerSecond;
                return Mathf.Clamp01(_costTimer / interval);
            }
        }

        [SerializeField] private bool _isRunning = false;

        protected override void Awake()
        {
            base.Awake();
            Cost = startCost;
        }
        
        private void Update()
        {
            if (!_isRunning || _cost >= maxCost)
            {
                _costTimer = 0;
                return;
            }
            _costTimer += Time.deltaTime;
            float interval = 1f / costPerSecond; 
            
            while (_costTimer >= interval) //렉걸려서 deltaTime이 1을 넘어선 경우, while문으로 처리.
            {
                _costTimer -= interval;
                Cost++;

                if (Cost >= maxCost)
                {
                    _costTimer = 0;
                    break;
                }
            }
        }
    }
}