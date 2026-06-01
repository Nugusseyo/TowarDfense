using _Scripts.Agent.Enemy;
using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/CitizenStateChange")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "CitizenStateChange", message: "citizen change [State]", category: "Events", id: "8608eac8c035f4e5defc85c71dfb89d2")]
public sealed partial class CitizenStateChange : EventChannel<CitizenState> { }

