using System;
using UnityEngine;

namespace _Script.Agent.Modules.BattleSystem
{
    //<주요 기능>

    /*  Operator의 Skill SP 카운팅
     *  상속받는 Operator Skill의 필수 정보를 담아주기
     */
    public abstract class AbstractPlayerSkill : MonoBehaviour
    {
        /*
        public delegate void SKillPointValueChange(float prev, float newValue);
        public event SKillPointValueChange OnSKillPointValueChange;

        [field: SerializeField] public GetSpType GetSkillPointType { get; private set; }        // 어떤 방식으로 SP를 얻는지

        [field: SerializeField] public int MaxSkillPoint { get; private set; }                  // Skill Point의 최대 상한선

        private float skillPoint;
        public float SkillPoint                                                                   // Skill을 발동하기 위한 Skill Point의 갯수
        {
            get => skillPoint;
            set
            {
                float newValue = Mathf.Clamp(value, 0, MaxSkillPoint);

                if (Mathf.Approximately(skillPoint, newValue)) return;

                OnSKillPointValueChange?.Invoke(skillPoint, newValue);
                skillPoint = newValue;
            }
        }
        [field: SerializeField] public float GetPointValue { get; private set; } = 1f;         // 1초에 얻는 SP의 양 또는 1타에 얻는 SP의 양

        protected Agent agent;

        public void SkillInitialize(ISkillModule skillModule)
        {

        }

        protected virtual void Update()
        {
            if (GetSkillPointType == GetSpType.SecondBase && agent)
            {
                SkillPoint += GetPointValue * Time.deltaTime;
            }
        }
    }

    
    */
    }
    public enum GetSpType
    {
        SecondBase,     // 1초당 n의 SP를 얻는다.
        AttackBase,     // 1타당 n의 SP를 얻는다.
        HitBase         // 1번 맞으면 n의 SP를 얻는다.
    }
}