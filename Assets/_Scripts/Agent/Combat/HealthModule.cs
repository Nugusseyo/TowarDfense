using _Script.Agent.Modules;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Agent.Combat
{
    public class HealthModule : MonoBehaviour, IModule
    {
        [field: SerializeField] public int MaxHealth { get; private set; } = 1000;
        private int _health;
        public int Health
        {
            get => _health;
            set
            {
                int nextHealth = Mathf.Clamp(value, 0, MaxHealth);
                
                if(Mathf.Approximately(_health, value)) 
                    return;

                _health = nextHealth;
            }
        }
        public float GetHealthNormal => Mathf.Clamp01((float)Health / MaxHealth);

        public bool IsDead { get; private set; } = false;

        private ModuleAgent _moduleAgent;

        public UnityEvent OnHit;
        public UnityEvent OnHeal;
        public UnityEvent OnDeath;
        public void Initialize(ModuleAgent moduleAgent)
        {
            _moduleAgent = moduleAgent;
            Debug.Assert(_moduleAgent != null, $"HealthModule인데 ModuleAgent가 없어요. Target : {gameObject.name}");
            Health = MaxHealth;
        }


        public void TakeDamage(int damage)
        {
            if (damage == 0 || damage <= 0) return;

            Health -= damage;
            if (Health <= 0)
            {
                OnDeath?.Invoke();
                IsDead = true;
                return;
            }
            OnHit?.Invoke();
        }

        public void TakeHeal(int heal)
        {
            if(heal == 0 || heal <= 0) return;
            
            Health += heal;
            OnHeal?.Invoke();
        }
        
    }
}
