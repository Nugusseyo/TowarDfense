using System;
using _Script.Camera;
using _Script.ScriptableObject;
using _Scripts.Agent;
using UnityEngine;

namespace _Script.Agent.Modules
{
    [RequireComponent(typeof(Animator))]
    public class LegacyAgentRenderer : MonoBehaviour, IModule, IRenderer, IAnimationTrigger
    {
        private Agent _agent;
        private Animator _animator;
        public void Initialize(ModuleAgent moduleAgent)
        {
            _agent = moduleAgent as Agent;
            _animator = GetComponent<Animator>();
            
        }

        private void SetAgentAngle(Vector3 newAngle)
        {
            transform.eulerAngles = newAngle;
        }

        public void PlayAnimation(int hash, int layer = -1, float normalizedTime = 0) => _animator.Play(hash, layer, normalizedTime);
        public void SetBool(AnimationHashSO hash, bool value) => _animator.SetBool(hash.AnimationHash, value);
        public void SetFloat(AnimationHashSO hash, float value) => _animator.SetFloat(hash.AnimationHash, value);
        public void SetInt(AnimationHashSO hash, int value) => _animator.SetInteger(hash.AnimationHash, value);
        public void SetTrigger(AnimationHashSO hash) => _animator.SetTrigger(hash.AnimationHash);
        
        #region Trigger

        public event Action OnAnimationEnd;
        public event Action OnAttackTrigger;
        public void ResetEndTrigger()
        {
            
        }

        public void ResetAttackTrigger()
        {
            
        }

        private void EndTrigger() => OnAnimationEnd?.Invoke();
        private void AttackTrigger() => OnAttackTrigger?.Invoke();

        #endregion
    }
}