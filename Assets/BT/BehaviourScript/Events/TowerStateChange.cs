using _Scripts.Agent.Tower;
using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/TowerStateChange")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "TowerStateChange", message: "Tower change [State]", category: "Events", id: "9320a4712da1220cd1c7b81ba8c98255")]
public sealed partial class TowerStateChange : EventChannel<TowerState> { }

