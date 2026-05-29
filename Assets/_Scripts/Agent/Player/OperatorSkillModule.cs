using System;
using UnityEngine;

namespace _Scripts.Agent.Player
{
    public class OperatorSkillModule : AgentSkillModule
    {
        public override void UseSkill()
        {
            base.UseSkill();
            if (_targetCaster == null || _agent == null)
            {
                Debug.LogError($"targetCaster 또는 agent가 존재하지 않지만, Skill을 사용하려 시도했습니다. Target : {gameObject.name}");
                return;
            }
            SkillData.UseSkill(_agent, _targetCaster);
        }
    }
}
