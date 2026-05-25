using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Script.Agent.CombatSystem
{
    public class OperatorTargetCaster : AbstractTargetCaster
    {
        [SerializeField] private Vector3 boxSize;

        public override bool CastEnemy(Collider owner)
        {
            List<Collider> hitList = new List<Collider>();
            attackTarget = new List<Collider>();
            foreach (Vector3 range in statModule.GetAttackRange())
            {
                Vector3 castPosition = _grid.WorldToCell(Owner.transform.position * 2) +
                                       range * _grid.cellSize.x;
                hitList.AddRange( Physics.OverlapBox(castPosition, boxSize / 2, Quaternion.identity, targetLayer).ToList());
            }

            hitList.Remove(owner);
            if (hitList.Count == 0)
            {
                attackTarget.Clear();
                return false;
            }
            hitList = hitList.Distinct().ToList();
            foreach (Collider hitTarget in hitList)
            {
                if (hitTarget.TryGetComponent<IDamageable>(out IDamageable damageable) && hitTarget.CompareTag("Enemy"))
                {
                    attackTarget.Add(hitTarget);
                }
            }
            attackTarget = attackTarget.Distinct().ToList();
            return attackTarget.Count > 0;
            
        }
        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.red;
                foreach (Vector3 range in statModule.GetAttackRange())
                {
                    Gizmos.DrawWireCube(_grid.WorldToCell(Owner.transform.position * 2)+ range * _grid.cellSize.x, boxSize);
                }
            }
        }
    }
}