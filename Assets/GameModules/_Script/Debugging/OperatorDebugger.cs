using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using _Script;
using _Script.Agent.CombatSystem;
using _Script.Agent.Modules.StatSystem;
using _Script.Agent.Operator;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class OperatorDebugger : MonoBehaviour
{
    [SerializeField] private PlayerOperator target;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            target.ChangePlayerState(PlayerStateEnum.START);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            OperatorTargetCaster targetCaster = target.GetComponentInChildren<OperatorTargetCaster>();
            targetCaster.Initialize(target);
            //targetCaster.CastEnemy();
            List<Vector3Int> list = target.GetModule<IStatModule>().GetAttackRange();
            foreach (Vector3 result in list)
            {
                Debug.Log(result);
            }
        }
    }
}
