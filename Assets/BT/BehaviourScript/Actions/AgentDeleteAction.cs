using _Scripts.Agent;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Object = UnityEngine.Object;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Agent Delete", story: "[Agent] delete [Second]", category: "Action", id: "36b090228370a5870cd1ddcec092fc3c")]
public partial class AgentDeleteAction : Action
{
    [SerializeReference] public BlackboardVariable<Agent> Agent;
    [SerializeReference] public BlackboardVariable<float> Second;

    private float _startTime;
    
    protected override Status OnStart()
    {
        if (Agent.Value == null)
            return Status.Failure;
        
        _startTime = Time.time;
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(_startTime + Second > Time.time) return Status.Running;

        Object.Destroy(Agent.Value.gameObject);
        return Status.Success;
    }
}

