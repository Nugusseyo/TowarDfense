using UnityEngine;

namespace _Scripts.Agent
{
    public interface IAgentRenderer
    {
        Animator Animator { get; }
        void PlayFadeAcrossClip(int clipSource, float duration);
        void PlayClip(int clipSource);
        void SetAnimatorFloat(int id, float value);
    }
}