using System;
using System.Collections;
using _Script.Agent.FSM;
using _Script.Agent.FSM.Tags;
using _Script.Agent.Modules.StatSystem;
using _Script.Agent.Operator;
using _Script.ScriptableObject;
using _Scripts.Agent;
using UnityEngine;

namespace _Script.Agent.Modules.BattleSystem
{
    public abstract class AbstractPlayerSkillModule : MonoBehaviour, IModule, ILateInitialize, ISkillModule
    {
        protected ModuleAgent _moduleAgent;
        protected Agent _agent;
        protected PlayerOperator _operator;
        protected IAnimationTrigger _trigger;

        public event Action OnSkillStart;
        public event Action OnSkillEnd;

        //IRenderer 만들면 여기서 공속 제어, Use Skill에서 Clip 제어 해줘야 한다.

        protected IStatModule _statModule;

        protected float _currentAttackDamage = 1f;
        protected float _currentAttackSpeed = 1f;
        protected float _currentHealth = 1f;
        protected float _currentDefensive = 1f;
        protected float _currentMagicResistance = 1f;

        public bool IsUsingSkill { get; protected set; }
        public bool CanGetSp { get; protected set; } = true;

        public float GetSpValue { get; private set; } = 1f;

        protected float _currentSkillPoint;

        public float CurrentSkillPoint
        {
            get => _currentSkillPoint;
            set
            {
                float newSp = Mathf.Clamp(value, 0, PlayerUltimateSkill.spValue);
                if (!Mathf.Approximately(newSp, CurrentSkillPoint))
                {
                    _currentSkillPoint = newSp;
                }
            }
        }

        public float NormalizedSkillPoint => Mathf.Clamp01(CurrentSkillPoint / PlayerUltimateSkill.spValue);

        #region Stat 선언부

        [field: SerializeField] public StatSO AttackDamageStat { get; private set; }
        [field: SerializeField] public StatSO AttackSpeedStat { get; private set; }
        [field: SerializeField] public StatSO HealthStat { get; private set; }
        [field: SerializeField] public StatSO DefensiveStat { get; private set; }
        [field: SerializeField] public StatSO MagicResistanceStat { get; private set; }

        [field: SerializeField] public AnimationHashSO AttackSpeedHash { get; private set; }

        #endregion

        [field: SerializeField] public NormalAttackDataSO PlayerNormalAttack { get; private set; }
        [field: SerializeField] public UltimateSkillDataSO PlayerUltimateSkill { get; private set; }

        public SkillDataSO CurrentSkill { get; private set; }
        private DamageData damageData;

        public void Initialize(ModuleAgent moduleAgent)
        {
            _moduleAgent = moduleAgent;
            _agent = moduleAgent as Agent;
            
            Debug.Assert(_moduleAgent != null && _agent != null, $"{gameObject.name} ModuleAgent or Agent is Null!!");

            _statModule = _agent.GetModule<IStatModule>();

            _trigger = _agent.GetModule<IAnimationTrigger>();
            Debug.Assert(_trigger != null, $"{gameObject.name}에 IAnimationTrigger가 존재하지 않습니다!");

            OnSkillStart += HandleOperatorSkillStart;
            OnSkillEnd += HandleOperatorSkillEnd;

            CurrentSkill = PlayerNormalAttack;
            damageData.Dealer = moduleAgent;
            
            StartGetSp();
        }

        private void StartGetSp()
        {
            foreach (GetSpType spType in PlayerUltimateSkill.spType)
            {
                switch (spType)
                {
                    case GetSpType.AttackBase :
                        _trigger.OnAttackTrigger += HandleGetSp;
                        break;
                    case GetSpType.HitBase :
                        _operator.OnHit.AddListener(HandleGetSp);
                        break;
                    case GetSpType.SecondBase :
                        StartCoroutine(GetSpBySecondCoroutine());
                        break;
                }
            }
        }

        private IEnumerator GetSpBySecondCoroutine()
        {
            while (!Mathf.Approximately(CurrentSkillPoint, PlayerUltimateSkill.spValue))
            {
                if (_operator.GetCurrentState() is ICanGetSp)
                {
                    CurrentSkillPoint += GetSpValue * Time.deltaTime;
                }
                yield return null;
            }
            
        }

        private void HandleGetSp()
        {
            if(_operator.GetCurrentState() is ICanGetSp)
                CurrentSkillPoint += GetSpValue;
        }
        
        public void SetCanGetSp(bool canGetSp) => CanGetSp = canGetSp;

        public void LateInitialize(ModuleAgent moduleAgent)
        {
            if (_statModule != null)
            {
                _currentAttackDamage = _statModule.Subscribe(AttackDamageStat.Index, HandleAttackChanged, _currentAttackDamage);
                _currentAttackSpeed = _statModule.Subscribe(AttackSpeedStat.Index, HandleAttackSpeedChanged, _currentAttackSpeed);
                _currentHealth = _statModule.Subscribe(HealthStat.Index, HandleHealthChanged, _currentHealth);
                _currentDefensive = _statModule.Subscribe(DefensiveStat.Index,HandleDefensiveChanged, _currentDefensive);
                _currentMagicResistance = _statModule.Subscribe(MagicResistanceStat.Index, HandleMagicResistanceChanged, _currentMagicResistance);
            }
            
            //IRenderer에 AttackSpeed값 SetFloat로 넣어줘야됨.
        }

        private void OnDestroy()
        {
            if (_statModule != null)
            {
                _statModule.UnSubscribe(AttackDamageStat.Index, HandleAttackChanged);
                _statModule.UnSubscribe(AttackSpeedStat.Index, HandleAttackSpeedChanged);
                _statModule.UnSubscribe(HealthStat.Index, HandleHealthChanged);
                _statModule.UnSubscribe(DefensiveStat.Index,HandleDefensiveChanged);
                _statModule.UnSubscribe(MagicResistanceStat.Index, HandleMagicResistanceChanged);
            }
            
            OnSkillStart -= HandleOperatorSkillStart;
            OnSkillEnd -= HandleOperatorSkillEnd;
        }

        public virtual void UseSkill(GameObject target = null)
        {
            if (!CanUseSkill()) return;

            if (CurrentSkill.skillParticleEffect != null)
            {
                //CurrentSkill.skillParticleEffect.StartParticle();
            }
            IsUsingSkill = true;
            OnSkillStart?.Invoke();
        }

        public virtual void StopSkill()
        {
            if (CurrentSkill.skillParticleEffect != null)
            {
                
            }
            IsUsingSkill = false;
            OnSkillEnd?.Invoke();
        }

        public virtual bool CanUseSkill(GameObject target = null)
        {
            return _operator.GetCurrentState() is ICanUseSkill && NormalizedSkillPoint >= 1 && !IsUsingSkill;
        }
        
        public void InvokeAttackEnd() => OnSkillEnd?.Invoke();

        public DamageData GetDamageBase()
        {
            damageData.Amount = _currentAttackDamage;
            damageData.Condition = CurrentSkill.Condition;
            damageData.KnockbackForce = CurrentSkill.knockbackPower;
            damageData.Type = CurrentSkill.damageType;
            //damageData.hitEffect = CurrentSkill.hitParticleEffect;
            return damageData;
        }

        private void HandleOperatorSkillStart()
        {
            CurrentSkill = PlayerUltimateSkill;
            //오퍼레이터의 능력치를 조정한다.
            foreach (StatSO statSO in PlayerUltimateSkill.effectTarget)
            {
                _statModule.AddBuffer(statSO.Index, this, statSO.BufferValue);
                _statModule.AddModifier(statSO.Index, this, statSO.ModifyValue);
            }
            
            if(PlayerUltimateSkill.attackRange != null) _statModule.SetAttackRange(PlayerUltimateSkill.attackRange);

            if (!PlayerUltimateSkill.isBullet)
            {
                StartCoroutine(SkillPointDecrease());
            }
            else
            {
                _trigger.OnAttackTrigger += HandleBulletOnAttack;
            }
        }

        private IEnumerator SkillPointDecrease()
        {
            //여기서 Sp Bar 색상 노란색으로 변경
            while (CurrentSkillPoint > 0)
            {
                CurrentSkillPoint -= Time.deltaTime;
                yield return null;
            }

            StopSkill();
        }

        private void HandleBulletOnAttack()
        {
            CurrentSkillPoint--;
            if (CurrentSkillPoint == 0)
            {
                _trigger.OnAttackTrigger -= HandleBulletOnAttack;
                StopSkill();
            }
        }
            

        private void HandleOperatorSkillEnd()
        {
            CurrentSkill = PlayerNormalAttack;
            if (_statModule != null && PlayerUltimateSkill.effectTarget != null)
            {
                foreach(StatSO statSO in PlayerUltimateSkill.effectTarget)
                {
                    _statModule.RemoveBoth(statSO.Index,this);
                }
            }
        }

        #region Stat System Setting
        private void HandleAttackChanged(StatSO statSO, float currentValue, float previousValue) => _currentAttackDamage = currentValue;

        private void HandleAttackSpeedChanged(StatSO statSO, float currentValue, float previousValue)
        {
            //여기에 IRenderer속 AttackSpeed Hash값 넣어줘야됨
            _currentAttackSpeed = currentValue;
        }

        private void HandleHealthChanged(StatSO statSO, float currentValue, float previousValue) => _currentHealth = currentValue;
        private void HandleDefensiveChanged(StatSO statSO, float currentValue, float previousValue) => _currentDefensive = currentValue;
        private void HandleMagicResistanceChanged(StatSO statSO, float currentValue, float previousValue) => _currentMagicResistance = currentValue;

        public float GetAttackValue() => _currentAttackDamage;
        public float GetAttackSpeedValue() => _currentAttackSpeed;
        public float GetDefensiveValue() => _currentDefensive;
        public float GetMagicResistanceValue() => _currentMagicResistance;
        public float GetSkillPoint() => _currentSkillPoint;

        #endregion
    }
}