using _Scripts.Agent.Player;
using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/PlayerStateChange")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "PlayerStateChange", message: "operator [State] change", category: "Events", id: "0fa106dc02ea469e4efd83ba22c1ca41")]
public sealed partial class PlayerStateChange : EventChannel<OperatorStateEnum> { }

