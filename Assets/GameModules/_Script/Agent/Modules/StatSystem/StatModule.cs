using System.Collections.Generic;
using System.Linq;
using _Script.Agent.Modules.BattleSystem;
using _Script.ScriptableObject;
using UnityEngine;

namespace _Script.Agent.Modules.StatSystem
{
    public class StatModule : MonoBehaviour, IModule, IStatModule
    {
        public delegate void AttackRangeHandler(List<Vector3Int> newAttackRange, List<Vector3Int> oldRange);

        public event AttackRangeHandler OnAttackRangeChange; 
        
        
        [SerializeField] private AgentStatus[] agentStatus;
        
        private Dictionary<int, StatSO> _statDictionary = new Dictionary<int, StatSO>(); //나의 스탯이 들어있는 Dictionary
        
        private ModuleAgent _moduleAgent;
        private ISkillModule _skillModule;
        public List<Vector3Int> AttackRange { get; private set; }

        public void Initialize(ModuleAgent moduleAgent)
        {
            _moduleAgent = moduleAgent;
            _statDictionary = agentStatus.ToDictionary(stat => stat.TargetStat.Index, stat => stat.CreateStatSO());

            _skillModule = _moduleAgent.GetModule<ISkillModule>();
            Debug.Assert(_statDictionary.Count > 0, $"Stat {gameObject.name}에 아무런 스탯이 들어있지 않습니다!");
            Debug.Assert(_skillModule != null, $"{gameObject.name}의 SkillModule이 존재하지 않습니다!");

            AttackRange = _skillModule.PlayerNormalAttack.baseAttackRange;
        }

        public StatSO[] GetAllStat()
        {
            return _statDictionary.Values.ToArray();
        }

        public StatSO GetStatByIndex(int statIndex)
        {
            return _statDictionary.GetValueOrDefault(statIndex);
        }

        public bool TryGetValueByIndex(int statIndex, out StatSO stat)
        {
            return _statDictionary.TryGetValue(statIndex, out stat);
        }

        public void AddModifier(int index, object key, float value)
        {
            if (_statDictionary.ContainsKey(index))
            {
                _statDictionary[index].AddModifier(key, value);
                return;
            }
            Debug.LogError($"{gameObject.name}의 스탯 인덱스 [{index}]를 찾을 수 없습니다.");
        }

        public void AddBuffer(int index, object key, float value)
        {
            if (_statDictionary.ContainsKey(index))
            {
                _statDictionary[index].AddBuffer(key, value);
                return;
            }
            Debug.LogError($"{gameObject.name}의 스탯 인덱스 [{index}]를 찾을 수 없습니다.");
        }

        public void RemoveModifier(int index, object key)
        {
            if (_statDictionary.ContainsKey(index))
            {
                _statDictionary[index].RemoveModifier(key);
                return;
            }
            Debug.LogError($"{gameObject.name}의 스탯 인덱스 [{index}]를 찾을 수 없습니다.");
        }

        public void RemoveBuffer(int index, object key)
        {
            if (_statDictionary.ContainsKey(index))
            {
                _statDictionary[index].RemoveBuffer(key);
                return;
            }
            Debug.LogError($"{gameObject.name}의 스탯 인덱스 [{index}]를 찾을 수 없습니다.");
        }

        public void RemoveBoth(int index, object key)
        {
            if (_statDictionary.ContainsKey(index))
            {
                _statDictionary[index].RemoveBoth(key);
                return;
            }
            Debug.LogError($"{gameObject.name}의 스탯 인덱스 [{index}]를 찾을 수 없습니다.");
        }

        public void ClearBuff()
        {
            foreach (StatSO stat in _statDictionary.Values)
            {
                stat.ClearAll();
            }
        }

        public float Subscribe(int index, StatSO.ValueChangedHandler handler, float defaultValue) //구독하면 float값을 (스탯을) 계속 return해준다. 
        {
            if (_statDictionary.TryGetValue(index, out StatSO stat))
            {
                stat.OnValueChanged += handler;
                return stat.Value;
            }

            return defaultValue;
        }

        public void UnSubscribe(int index, StatSO.ValueChangedHandler handler)
        {
            if (_statDictionary.TryGetValue(index, out StatSO stat))
            {
                stat.OnValueChanged -= handler;
            }
        }

        public void AddAttackRange(List<Vector3Int> newAttackRange)
        {
            List<Vector3Int> rangeList = new List<Vector3Int>(AttackRange);
            rangeList.AddRange(newAttackRange);
            rangeList = rangeList.Distinct().ToList(); //List에 중복값이 있을 경우를 대비해 중복값을 제거하고 다시 리스트 재생성
            
            if (AttackRange == rangeList) return;
            
            List<Vector3Int> prevList = new List<Vector3Int>(AttackRange);
            AttackRange = rangeList;
            OnAttackRangeChange?.Invoke(AttackRange, prevList);
        }

        public void RemoveAttackRange(List<Vector3Int> removeBuffedRange, bool isSaveBaseAttackRange)
        {
            List<Vector3Int> prevList = new List<Vector3Int>(AttackRange);
            foreach(Vector3Int removeTarget in removeBuffedRange)
            {
                AttackRange.Remove(removeTarget);
            }

            if (isSaveBaseAttackRange)
            {
                AttackRange.AddRange(_skillModule.PlayerNormalAttack.baseAttackRange);
            }

            OnAttackRangeChange?.Invoke(AttackRange, prevList);
        }

        public void SetAttackRange(List<Vector3Int> newAttackRange)
        {
            Debug.Assert(newAttackRange.Count != 0, $"{gameObject.name} : SetAttackRange Exception : New AttackRange의 크기가 0입니다.");
            
            List<Vector3Int> prevList = new List<Vector3Int>(AttackRange);
            AttackRange = newAttackRange;
            OnAttackRangeChange?.Invoke(AttackRange, prevList);
        }

        public List<Vector3Int> GetAttackRange() => AttackRange;

        public List<Vector3Int> GetBaseAttackRange() => _skillModule.PlayerNormalAttack.baseAttackRange;

        public void ResetAttackRange()
        {
            List<Vector3Int> prevList = new List<Vector3Int>(AttackRange);
            AttackRange = _skillModule.PlayerNormalAttack.baseAttackRange;
            OnAttackRangeChange?.Invoke(AttackRange, prevList);
        }
        
        public bool TryGetStat(int index, out StatSO outStat) => _statDictionary.TryGetValue(index, out outStat);
    }
}