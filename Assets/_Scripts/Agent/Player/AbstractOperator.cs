using System;
using Unity.Behavior;
using UnityEngine;

namespace _Scripts.Agent.Player
{
    public class AbstractOperator : Agent
    {
        private void Start()
        {
            //BT는 Awake에서 만들기 시작하므로 Start에서 만들어주자.
            AgentBT.SetVariableValue(OperatorStrings.Operator, this);
            
            //임시 AttackModuleCode 구역
            if (!GetVariable(OperatorStrings.ChanneEvent, out BlackboardVariable<PlayerStateChange> stateChannel))
            {
                Debug.LogError($"StateChannel이 존재하지 않습니다. Target : {gameObject.name}");
                return;
            }
            _playerStateChange = stateChannel.Value;
            //End
        }

        #region 임시 AttackModule Code

        [SerializeField] private LayerMask targetLayer;
        
        public override bool TryCasting()
        {
            if (TargetCaster.SearchTargetSphere(AgentStatusSO.DetectRadius, targetLayer))
            {
                return true;
            }

            return false;
        }

        private PlayerStateChange _playerStateChange;

        public void UseSkill()
        {
            _playerStateChange.SendEventMessage(OperatorStateEnum.SKILL);
            Debug.Log("Use Skill!!!");
        }
        

        private void OnDrawGizmosSelected()
        {
            if (AgentStatusSO == null) return;
            
            Gizmos.color = Color.aquamarine;
            Gizmos.DrawWireSphere(transform.position, AgentStatusSO.DetectRadius);
        }
        #endregion
    }

    public static class OperatorStrings
    {
        public const string Operator = "Operator";
        public const string ChanneEvent = "StateChangeEvent";
    }
}