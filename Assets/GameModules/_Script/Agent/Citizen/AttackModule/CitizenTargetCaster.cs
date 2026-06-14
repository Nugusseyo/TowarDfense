using _Script.Agent.Modules;
using _Scripts.Agent;
using UnityEngine;

namespace GameModules._Script.Agent.Citizen.AttackModule
{
    public class CitizenTargetCaster : MonoBehaviour, IModule, ITargetCaster
    {
        public Collider[] SucceedColliders { get; }
        public int HitCount { get; }
        public bool SearchTargetSphere(float radius, bool isResetOriginColliders = false)
        {
            return false;
        }

        public bool SearchTargetSphere(Vector3 pos, float radius, bool isResetOriginColliders = false)
        {
            return false;
        }

        public bool SearchTargetBox(Vector3 centerOffset, Vector3 size)
        {
            return false;
        }

        public void ResetTargets()
        {
        }

        public void Initialize(ModuleAgent moduleAgent)
        {
            
        }
    }
}
