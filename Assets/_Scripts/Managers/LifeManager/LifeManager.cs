using System;
using _Script.Tools.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _Scripts.Managers.LifeManager
{
    public class LifeManager : MonoSingleton<LifeManager>
    {
        public UnityEvent OnLifeDamaged;
        public Action<int> OnLifeChanged;
        [field:SerializeField] public int MaxLife { get; private set; }

        private int _currentLife;

        public int CurrentLife
        {
            get => _currentLife;
            set
            {
                int life = Mathf.Clamp(value, 0, MaxLife);
                
                if (life == CurrentLife) 
                    return;
                
                if (life < _currentLife) 
                    OnLifeDamaged?.Invoke();
                
                
                OnLifeChanged?.Invoke(life);
                
                _currentLife = life;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _currentLife = MaxLife;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            OnLifeDamaged.RemoveAllListeners();
            OnLifeChanged = null;
        }
    }
}
