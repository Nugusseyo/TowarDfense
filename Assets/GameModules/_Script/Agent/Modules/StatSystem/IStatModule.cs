using System.Collections.Generic;
using _Script.ScriptableObject;
using UnityEngine;

namespace _Script.Agent.Modules.StatSystem
{
    public interface IStatModule
    {
        StatSO[] GetAllStat();
        StatSO GetStatByIndex(int statIndex);
        bool TryGetValueByIndex(int statIndex, out StatSO stat);
        void AddModifier(int index, object key, float value);
        void AddBuffer(int index, object key, float value);
        void RemoveModifier(int index, object key);
        void RemoveBuffer(int index, object key);
        void RemoveBoth(int index, object key);
        float Subscribe(int index, StatSO.ValueChangedHandler handler, float defaultValue);
        void UnSubscribe(int index, StatSO.ValueChangedHandler handler);
        void AddAttackRange(List<Vector3Int> newAttackRange);
        void RemoveAttackRange(List<Vector3Int> removeBuffedRange, bool isSaveBaseAttackRange);
        void SetAttackRange(List<Vector3Int> newAttackRange);
        List<Vector3Int> GetAttackRange();

        List<Vector3Int> GetBaseAttackRange();

        void ResetAttackRange();
        
        bool TryGetStat(int index, out StatSO outStat);
        void ClearBuff();
    }
}