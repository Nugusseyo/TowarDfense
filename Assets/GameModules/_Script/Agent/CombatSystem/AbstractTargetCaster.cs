using System.Collections.Generic;
using _Script.Agent.Modules;
using _Script.Agent.Modules.StatSystem;
using GameModules._Script.Agent.CombatSystem;
using UnityEngine;

namespace _Script.Agent.CombatSystem
{
    public abstract class AbstractTargetCaster : MonoBehaviour, IModule, IOperatorTargetCaster
    {
        protected Agent Owner;
        protected IStatModule statModule;

        protected Grid _grid;

        [SerializeField] protected int maxCount;
        [SerializeField] protected LayerMask targetLayer;
        
        protected List<Collider> attackTarget = new List<Collider>();
        public void Initialize(ModuleAgent moduleAgent)
        {
            Owner = moduleAgent as Agent;
            statModule = moduleAgent.GetModule<IStatModule>();
            Debug.Assert(statModule != null, $"{gameObject.name} IStatModule is Null!!");
            _grid = FindFirstObjectByType<Grid>();
        }
        public abstract bool CastEnemy(Collider owner);

        public List<Collider> GetAttackTarget() => attackTarget;
    }
}