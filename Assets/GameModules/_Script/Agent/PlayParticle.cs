using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GameModules._Script.Agent
{
    public class PlayParticle : MonoBehaviour
    {
        public UnityEvent<Transform> PlayParticleEvent;

        private List<ParticleSystem> _particles = new List<ParticleSystem>();
        private void Awake()
        {
            _particles = GetComponentsInChildren<ParticleSystem>().ToList();
            PlayParticleEvent.AddListener(HandlePlayParticleEvent);
        }
        
        public void Play(Transform trm) => PlayParticleEvent.Invoke(trm);

        private void OnDestroy()
        {
            PlayParticleEvent.RemoveListener(HandlePlayParticleEvent);
        }

        private void HandlePlayParticleEvent(Transform trm)
        {
            foreach (ParticleSystem particle in _particles)
            {
                Vector3 position = new Vector3(trm.position.x, trm.position.y + 0.5f, trm.position.z);
                particle.transform.position = position;
                particle.Play();
            }
        }
    }
}
