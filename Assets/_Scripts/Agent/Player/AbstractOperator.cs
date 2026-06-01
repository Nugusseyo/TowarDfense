using System;
using _Scripts.Managers.Board;
using Unity.Behavior;
using UnityEngine;

namespace _Scripts.Agent.Player
{
    public class AbstractOperator : Agent
    {
        public PlayerStateChange PlayerStateChange => _playerStateChange;
        private PlayerStateChange _playerStateChange;

        private void OnEnable()
        {
            //BT는 Awake에서 만들기 시작하므로 Start에서 만들어주자. //Start에서 하니까 노드가 일찍 시작해서 안된다. OnEnable로 옮기자;;
            AgentBT.SetVariableValue(OperatorStrings.Operator, this);

            //임시 AttackModuleCode 구역
            if (!GetVariable(OperatorStrings.ChannelEvent, out BlackboardVariable<PlayerStateChange> stateChannel))
            {
                Debug.LogError($"StateChannel이 존재하지 않습니다. Target : {gameObject.name}");
                return;
            }

            _playerStateChange = stateChannel.Value;
            //End
        }


        

        public override void OnDeath()
        {
            Debug.Log("OnDeath Start");
            BoardManager.Instance.RemoveDictionary(this);
            _playerStateChange.SendEventMessage(OperatorStateEnum.DEAD);
        }
    }
    public static class OperatorStrings
    {
        public const string Operator = "Operator";
        public const string ChannelEvent = "StateChangeEvent";
    }
}