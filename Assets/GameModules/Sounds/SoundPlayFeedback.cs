using _Script.ScriptableObject.Event;
using GameLib.SoundSystem;
using UnityEngine;

namespace GameModules.Sounds
{
    public class SoundPlayFeedback : MonoBehaviour
    {
        [field: SerializeField] public EventChannelSO SoundEventChannel { get; private set; }
        [field: SerializeField] public SoundClipSO SoundClip { get; private set; }

        public void PlaySound()
        {
            SoundEventChannel.RaiseEvent(SoundSystemEvents.PlaySoundEvent.Init(transform.position, SoundClip));
        }
    }
}
