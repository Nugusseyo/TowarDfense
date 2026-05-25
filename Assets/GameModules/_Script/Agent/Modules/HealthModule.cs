using System;
using System.Collections.Generic;
using _Script.Agent.Modules.StatSystem;
using _Script.ScriptableObject;
using _Script.ScriptableObject.Event;
using UnityEngine;

namespace _Script.Agent.Modules
{
    public class HealthModule : MonoBehaviour, IModule, ILateInitialize
    {
        public delegate void HealthChanged(float prevHealth, float currentHealth, float max); //순서 조심하기. Health가 Change 된 경우 발생하는 Event
        public event HealthChanged OnHealthChanged;

        [SerializeField] private StatSO healthStatSO; // Subscribe 기능을 써주기 위함 (인덱스만 들고오기) 모든 스탯은 StatModule에서 관리된다.
        
        private float _currentHealth;
        private ModuleAgent _moduleAgent;
        private IStatModule _statModule;

        [field: SerializeField] public float MaxHealth { get; private set; } = 2000f;

        public float CurrentHealth
        {
            get => _currentHealth;
            set
            {
                float before = _currentHealth;
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
                if (!Mathf.Approximately(_currentHealth, before))
                {
                    OnHealthChanged?.Invoke(before, _currentHealth, MaxHealth);
                }
            }
        }

        public void Initialize(ModuleAgent moduleAgent)
        {
            _moduleAgent = moduleAgent;
            _statModule = moduleAgent.GetModule<IStatModule>();
        }

        public void LateInitialize(ModuleAgent moduleAgent)
        {
            if (_statModule != null)
            {
                MaxHealth = _statModule.Subscribe(healthStatSO.Index, HandleMaxHealthChanged, MaxHealth);
            }
            _currentHealth = MaxHealth;
        }

        private void OnDestroy()
        {
            if (_statModule != null)
            {
                _statModule.UnSubscribe(healthStatSO.Index, HandleMaxHealthChanged);
            }
        }

        private void HandleMaxHealthChanged(StatSO statSo, float currentValue, float previousValue)
        {
            float healthDifference = currentValue - previousValue;
            MaxHealth = currentValue;
            _currentHealth += healthDifference;
            _currentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        }

        public void GetDamage(float damage)
        {
            CurrentHealth -= damage;
            //여기서 현재 State 들고와서 걸린 상태이상에 따라 데미지 적용시키기
        }

        public void ResetHealth()
        {
            _currentHealth = MaxHealth;
        }
    }
}