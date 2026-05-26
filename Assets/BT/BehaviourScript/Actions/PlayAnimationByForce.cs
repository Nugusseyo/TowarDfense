using _Script.ScriptableObject;
using _Scripts.Agent.Player;
using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/PlayAnimationByForce")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "PlayAnimationByForce", message: "play animation [Clip] by force", category: "Events", id: "3a9de61889e5ebc5374036fea4d51fcc")]
public sealed partial class PlayAnimationByForce : EventChannel<AnimationHashSO> { }

