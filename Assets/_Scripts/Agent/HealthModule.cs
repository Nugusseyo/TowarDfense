using _Script.Agent.Modules;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Agent
{
    public class HealthModule : MonoBehaviour, IModule
    {
        //[SerializeField]
        //Max hp, min hp 필요
        
        private float _health;
        public float Health
        {
            get => _health;
            set
            {
                //if()
            }
        }

        private ModuleAgent _moduleAgent;

        public UnityEvent OnHit;
        public UnityEvent OnHeal;
        public UnityEvent OnDeath;
        public void Initialize(ModuleAgent moduleAgent)
        {
            _moduleAgent = moduleAgent;
            Debug.Assert(_moduleAgent != null, $"HealthModule인데 ModuleAgent가 없어요. Target : {gameObject.name}");
        }


        public void TakeDamage(float damage)
        {
            if (Mathf.Approximately(0, damage)) return;

            Health -= damage;
            if (Health <= 0)
            {
                OnDeath?.Invoke();
                return;
            }
            OnHit?.Invoke();
        }

        public void TakeHeal(float heal)
        {
            if(Mathf.Approximately(0, heal)) return;
            
            Health += heal;
            OnHeal?.Invoke();
        }
        
    }
}
