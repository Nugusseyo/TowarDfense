using _Script.Agent.CombatSystem;
using _Script.Agent.FSM.Tags;
using _Script.Agent.Modules;
using _Script.Agent.Modules.BattleSystem;
using _Script.Agent.Operator;
using _Script.ScriptableObject;
using UnityEngine;

namespace _Script.Agent.FSM.State
{
    public class PlayerAttackState : AbstractPlayerState, ICanDamageable
    {
        public PlayerAttackState(Agent agent, AnimationHashSO hash) : base(agent, hash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _renderer.PlayAnimation(_animationHash.AnimationHash);
            _trigger.OnAttackTrigger += HandleOperatorAttack;
            _trigger.OnAnimationEnd += HandleOperatorReset;
        }

        private void HandleOperatorReset()
        {
            _playerOperator.ChangePlayerState(PlayerStateEnum.IDLE);
        }

        private void HandleOperatorAttack()
        {
            int counter = 0;
            if (_playerOperator.touchedAgentList.Count == 0) //터치한 Enemy가 없는 경우.
            {
                if (TargetCaster.CastEnemy(_playerOperator.Collider))
                {
                    foreach (Collider target in TargetCaster.GetAttackTarget())
                    {
                        if (target.TryGetComponent<IDamageable>(out IDamageable damageable) && target != _playerOperator.Collider)
                        {
                            DamageData damageData = _skillModule.GetDamageBase();
                            damageable.GetDamage(damageData);
                            //damageData.hitEffect.transform.position = target.transform.position;
                            //damageData.hitEffect.GetComponent<ParticleEffect>().StartParticle();
                        }
                        counter++;
                        if (counter >= _playerOperator.MaxAttackCount) break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < _playerOperator.touchedAgentList.Count; i++) //터치한 Enemy가 존재하는 경우.
                {
                    if (_playerOperator.touchedAgentList[i].TryGetComponent<IDamageable>(out IDamageable damageable))
                    {
                        DamageData damageData = _skillModule.GetDamageBase();
                        damageable.GetDamage(damageData);
                        //damageData.hitEffect.transform.position = _playerOperator.touchedAgentList[i].transform.position;
                        //damageData.hitEffect.GetComponent<ParticleEffect>().StartParticle();
                    }
                    counter++;
                    if (_playerOperator.touchedAgentList.Count < i + 1 || _playerOperator.MaxAttackCount <= counter) break;
                }
            }
            
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
            _trigger.OnAttackTrigger -= HandleOperatorAttack;
            _trigger.OnAnimationEnd -= HandleOperatorReset;
        }
    }
}