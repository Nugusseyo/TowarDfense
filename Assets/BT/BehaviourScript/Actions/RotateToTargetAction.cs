using _Script.Agent.Operator;
using System;
using _Scripts.Agent;
using _Scripts.Agent.Player;
using GameModules._Script.Agent;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Rotate to Target", story: "[Agent] rotate to currentTarget", category: "Action", id: "89b0374032fe7ea8fd5a24518596b019")]
public partial class RotateToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<Agent> Agent;

    //private float _rotateTime = 0.4f;
    //private float _startTime;

    private Transform _targetTrm;
    
    private IAgentAttackModule _attackModule;
    
    protected override Status OnStart()
    {
        if (Agent.Value == null)
            return Status.Failure;

        _attackModule = Agent.Value.GetModule<IAgentAttackModule>();

        if (_attackModule == null)
        {
            Debug.Log("아니 어택 모듈이 없잖아여");
            return Status.Failure;
        }

        //_startTime = Time.time;
        
        Debug.Log("Start Time");
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        //if (_startTime + _rotateTime < Time.time) return Status.Success;
        if (_attackModule?.AttackTargetList == null || _attackModule.AttackTargetList.Count == 0)
        {
            _targetTrm = null;
        }
        else
        {
            Agent firstTarget = _attackModule.AttackTargetList[0];
            _targetTrm = firstTarget != null ? firstTarget.transform : null;
        }
        
        Vector3 direction;
        if (_targetTrm == null) //대상이 없다;;
            direction = Vector3.forward;
        //정면이나 보자.
        else
            direction = _targetTrm.position
                        - Agent.Value.transform.position;
        direction.y = 0;
        direction.Normalize();
        
        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            Agent.Value.transform.rotation = Quaternion.Lerp(
                Agent.Value.transform.rotation, 
                rotation, 
                10 * Time.deltaTime
            );
        }
        return Status.Running;
    }
}

