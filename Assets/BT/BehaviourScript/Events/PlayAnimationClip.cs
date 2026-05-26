using _Script.ScriptableObject;
using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/PlayAnimationClip")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "PlayAnimationClip", message: "play [AnimationClip]", category: "Events", id: "bbc33049c20f86ce5de157405916c563")]
public sealed partial class PlayAnimationClip : EventChannel<AnimationHashSO> { }

