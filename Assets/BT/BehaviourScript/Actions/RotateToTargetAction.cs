using _Script.Agent.Operator;
using System;
using _Scripts.Agent;
using _Scripts.Agent.Combat;
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
    private HealthModule _healthModule;
    private Transform _visual;
    
    protected override Status OnStart()
    {
        if (Agent.Value == null)
            return Status.Failure;

        _attackModule = Agent.Value.GetModule<IAgentAttackModule>();
        _healthModule = Agent.Value.GetModule<HealthModule>();
        _visual = Agent.Value.GetModule<IAgentRenderer>().Animator.gameObject.transform;

        if (_attackModule == null)
        {
            Debug.Log("아니 어택 모듈이 없잖아여");
            return Status.Failure;
        }

        if (_healthModule == null)
        {
            Debug.Log("Health Module 누락됨");
            return Status.Failure;
        }

        //_startTime = Time.time;
        
        Debug.Log("Start Time");
        
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (_attackModule.AttackTargetList == null || _attackModule.AttackTargetList.Count == 0)
        {
            _targetTrm = null;
        }
        else
        {
            Agent firstTarget = _attackModule.AttackTargetList[0];
            _targetTrm = firstTarget != null ? firstTarget.transform : null;
        }
        
        if (_targetTrm == null || _healthModule == null)
        {
            return Status.Running;
        }

        if (_healthModule.IsDead)
            return Status.Failure;
        
        Vector3 direction = _targetTrm.position - Agent.Value.transform.position;
        direction.y = 0;
        
        if (direction != Vector3.zero)
        {
            direction.Normalize();
            Quaternion rotation = Quaternion.LookRotation(direction);
            _visual.rotation = Quaternion.Lerp(
                _visual.rotation, 
                rotation, 
                20 * Time.deltaTime
            );
        }
        
        return Status.Running;
    }
}

