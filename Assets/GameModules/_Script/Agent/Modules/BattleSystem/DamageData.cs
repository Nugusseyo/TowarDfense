using _Script.Agent.CombatSystem;
using _Script.ScriptableObject;
using UnityEngine;

namespace _Script.Agent.Modules.BattleSystem
{
    public struct DamageData
    {
        public float Amount;
        public DamageType Type;
        public ConditionData[] Condition;
        public Vector2 KnockbackForce;
        public ModuleAgent Dealer;
        public GameObject hitEffect;
    }

    public struct ConditionData
    {
        public ConditionEnum TargetCondition;
        public float Amount;
    }

    public enum ConditionEnum
    {
        None,       //상태 이상 없음
        Resist,     //저항 [해로운 상태이상의 지속시간 절반으로 감소, none]
        Stun,       //기절 [순간적인 행동 불능, second]
        Slow,       //둔화 [이동 속도 감소 효과, %]
        Stop,       //정지 [이동 속도 80% 감소 효과, second]
        Bind,       //속박 [이동 불가 하지만 공격 및 스킬 가능, second]
        Silence,    //침묵 [특수 능력 봉쇄 효과, second]
        Weakness,   //취약 [아래 주석 참고]
        Chill,      //냉기 [공격속도 -30 및 빙결 부여, power]
        Freeze,     //빙결 [행동 불가 및 마법저항력 -15, second]
        Slumber,    //수면 [지속시간동안 행동 불능 및 무적, second]
        Fear,       //공포 [주위로 산개 및 행동 불능, second]
    }

    public enum DamageType
    {
        Physical,   //물리 데미지
        Magic,      //마법 데미지
        True,       //트루 데미지 (고정 데미지)
    }
}