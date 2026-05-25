using System;
using _Script.ScriptableObject;
using UnityEngine;

namespace _Script.Agent.Modules.BattleSystem
{
    public interface ISkillModule
    {
        event Action OnSkillStart;
        event Action OnSkillEnd;
        bool IsUsingSkill { get; }
        bool CanGetSp { get; }
        float CurrentSkillPoint { get; set; }
        float NormalizedSkillPoint { get; }
        StatSO AttackDamageStat { get; }
        StatSO AttackSpeedStat { get; }
        StatSO HealthStat { get; }
        StatSO DefensiveStat { get; }
        StatSO MagicResistanceStat { get; }
        AnimationHashSO AttackSpeedHash { get; }
        NormalAttackDataSO PlayerNormalAttack { get; }
        UltimateSkillDataSO PlayerUltimateSkill { get; }
        void SetCanGetSp(bool canGetSp);
        void UseSkill(GameObject target = null);
        void StopSkill();
        bool CanUseSkill(GameObject target = null);
        void InvokeAttackEnd();
        DamageData GetDamageBase();
        float GetAttackValue();
        float GetAttackSpeedValue();
        float GetDefensiveValue();
        float GetMagicResistanceValue();
        float GetSkillPoint();
    }
}