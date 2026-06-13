using UnityEngine;

namespace _Scripts.Agent
{
    public interface IAgentRenderer
    {
        GameObject GameObject { get; }
        Animator Animator { get; }
        void PlayFadeAcrossClip(int clipSource, float duration);
        void PlayClip(int clipSource);
        void SetAnimatorFloat(int id, float value);
        void PlayHitFlash(Color flashColor, float flashTime = 0.08f, int count = 2);
    }
}