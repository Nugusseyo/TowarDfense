using System;
using Unity.Behavior;
using UnityEngine;

namespace _Scripts.Agent.Enemy
{
    public class AbstractCitizen : Agent
    {
        private CitizenStateChange _stateChange;

        public ICitizenMover Mover { get; private set; }

        public override void TakeHeal(int healAmount)
        {
            base.TakeHeal(healAmount);
            //Debug.Log("나 힐받았어!!!");
        }

        protected override void Awake()
        {
            base.Awake();
            
            Mover = GetModule<ICitizenMover>();
            Debug.Assert(Mover != null, $"Citizen인데 Mover가 Null이면 어떡해요;;");
        }

        private void OnEnable()
        {
            AgentBT.SetVariableValue(CitizenStrings.Citizen, this);
            
            if (!GetVariable(CitizenStrings.StateChange, out BlackboardVariable<CitizenStateChange> stateChannel))
            {
                Debug.LogWarning("Citizen에 CitizenStateChannel이 존재하지 않습니다!");
                return;
            }
            _stateChange = stateChannel.Value;
        }

        public override void TakeDamage(int damageAmount)
        {
            base.TakeDamage(damageAmount);
        }

        public override void OnDeath()
        {
            Mover.NavAgent.isStopped = true;
            _stateChange.SendEventMessage(CitizenState.DEAD);
            Destroy(gameObject, 3f);
        }
    }

    public static class CitizenStrings
    {
        public const string StateChange = "CitizenStateChange";
        public const string Citizen = "Citizen";
    }
}
