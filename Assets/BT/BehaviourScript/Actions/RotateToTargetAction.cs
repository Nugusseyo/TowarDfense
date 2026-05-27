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
[NodeDescription(name: "Rotate to Target", story: "[Operator] rotate to currentTarget", category: "Action", id: "89b0374032fe7ea8fd5a24518596b019")]
public partial class RotateToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractOperator> Operator;

    //private float _rotateTime = 0.4f;
    //private float _startTime;

    private Transform _targetTrm;
    
    private IAgentAttackModule _attackModule;
    
    protected override Status OnStart()
    {
        if (Operator.Value == null)
            return Status.Failure;

        _attackModule = Operator.Value.GetModule<IAgentAttackModule>();

        if (_attackModule == null)
        {
            Debug.Log("아니 어택 모듈이 없잖아여");
            return Status.Failure;
        }

        //_startTime = Time.time;
        _targetTrm = _attackModule.AttackTargetList.Count == 0 ? null : _attackModule.AttackTargetList[0].transform;
        
        Debug.Log("Start Time");
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        //if (_startTime + _rotateTime < Time.time) return Status.Success;
        
        Vector3 direction;
        if (_targetTrm == null) //대상이 없다;;
            direction = Vector3.forward;
        //정면이나 보자.
        else
            direction = _targetTrm.position
                        - Operator.Value.transform.position;
        
        Quaternion rotation = Quaternion.LookRotation(direction.normalized);
        Operator.Value.transform.rotation =
            Quaternion.Lerp(Operator.Value.transform.rotation
                , rotation
                , 10 * Time.deltaTime);

        return Status.Running;
    }
}

