using System;
using UnityEngine;

namespace _Script.Agent.CombatSystem
{
    public class ParticleEffect : MonoBehaviour
    {
        private ParticleSystem _particle;

        private void Awake()
        {
            _particle = GetComponent<ParticleSystem>();
        }

        public void StartParticle()
        {
            _particle.Play();
        }

        public void StopParticle()
        {
            _particle.Stop();
        }
    }
}