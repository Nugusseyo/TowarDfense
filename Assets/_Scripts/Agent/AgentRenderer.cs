using _Script.Agent.Modules;
using UnityEngine;

namespace _Scripts.Agent
{
    public class AgentRenderer : MonoBehaviour, IModule, IAgentRenderer
    {
        private Agent _moduleAgent;
        private Animator _animator;
        public Animator Animator => _animator;
        public void Initialize(ModuleAgent moduleAgent)
        {
            _moduleAgent = moduleAgent as Agent;
            _animator = GetComponent<Animator>();
        }

        public void PlayFadeAcrossClip(int clipSource, float duration)
        {
            _animator.CrossFadeInFixedTime(clipSource, duration);
        }

        public void PlayClip(int clipSource)
        {
            _animator.Play(clipSource);
        }

        public void SetAnimatorFloat(int id, float value) => _animator.SetFloat(id, value);
    }
}
