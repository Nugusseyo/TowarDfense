using _Scripts.Agent.Player;
using Unity.Behavior;
using UnityEngine;

namespace _Scripts.Agent.Tower
{
    public class Tower : Agent
    {
        public TowerStateChange TowerStateChange => _towerStateChange;
        private TowerStateChange _towerStateChange;

        private void OnEnable()
        {
            AgentBT.SetVariableValue(TowerStrings.Tower, this);
            
            if (!GetVariable(TowerStrings.ChannelEvent, out BlackboardVariable<TowerStateChange> stateChannel))
            {
                Debug.LogError($"StateChannel이 존재하지 않습니다. Target : {gameObject.name}");
                return;
            }

            _towerStateChange = stateChannel.Value;
        }
    
        public override void OnDeath()
        {
        
        }
    }
    public static class TowerStrings
    {
        public const string Tower = "Tower";
        public const string ChannelEvent = "TowerStateChange";
    }
}

