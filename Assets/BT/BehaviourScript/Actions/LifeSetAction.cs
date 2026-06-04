using System;
using _Scripts.Managers.LifeManager;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Life Set", story: "game life add [Amount]", category: "Action", id: "4c5947d2534b19085de0e74ed16e1943")]
public partial class LifeSetAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Amount;

    protected override Status OnStart()
    {
        LifeManager.Instance.CurrentLife += Amount.Value;
        return Status.Success;
    }
}

