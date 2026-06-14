using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameModules._Script.Agent.Tower
{
    public class BulletFeedback : MonoBehaviour
    {
        public List<ParticleSystem> particles = new List<ParticleSystem>();

        public void PlayParticle(Vector3 position)
        {
            foreach (ParticleSystem particle in particles)
            {
                particle.transform.position = position;
                particle.Play();
            }
        }
    }
}
