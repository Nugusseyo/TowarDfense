using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Script.ScriptableObject
{
    [CreateAssetMenu(fileName = "new Stat SO", menuName = "Data/Combat/Stat SO", order = 0)]
    public class StatSO : IndexSO, ICloneable
    {
        public delegate void ValueChangedHandler(StatSO statSO, float currentValue, float previousValue); //스탯의 값이 변했을 때 Invoke되는 Event
        public  event ValueChangedHandler OnValueChanged;
        
        [field: SerializeField] public string Name { get; private set; }        //해당 스탯의 이름. 유저에게 보여지지 않음.
        [field: SerializeField] public Sprite Icon { get; private set; }        //스탯을 대표하는 이미지.
        [field: SerializeField] public string Desc { get; private set; }        //스탯에 대한 설명. 유저에게 보여짐.
        [field: SerializeField] public string ShowName { get; private set; }    //유저에게 보여지는 스탯의 이름.

        [SerializeField] private float baseValue;                               //스탯의 기본 수치값입니다. BaseValue 프로퍼티가 존재합니다.
        [SerializeField] private float minValue, maxValue;
        [SerializeField] private float bufMinValue = 0.1f, bufMaxValue = 10f;
        
        private Dictionary<object, float> _modifiedValueDictionary =  new Dictionary<object, float>(); //Dictionary를 이용해 버프를 받을 때 동일한 버프인지를 감지함.
        private Dictionary<object, float> _bufferValueDictionary =  new Dictionary<object, float>();   //Buffer를 Dictionary로 추가적으로 관리해줌.
        public float ModifyValue { get; private set; } = 0; //무언가의 효과로 인해 증가된 수치
        public float BufferValue { get; private set; } = 1; //최종적으로 n배 해주어 배율을 맞추어주는 용도.

        public float MaxValue => maxValue;
        public float MinValue => minValue;
        
        public float Value => Mathf.Clamp((baseValue + ModifyValue) * Mathf.Clamp(BufferValue, bufMinValue, bufMaxValue), MinValue, MaxValue); //해당 스탯의 수치입니다.
        public bool IsMaxValue => Mathf.Approximately(Value, MaxValue);
        public bool IsMinValue => Mathf.Approximately(Value, MinValue);

        public float BaseValue
        {
            get => baseValue;
            set
            {
                float prevValue = Value;
                baseValue = Mathf.Clamp(value, MinValue, MaxValue);
                TryInvokeValueChangeEvent(Value, prevValue);
            }
        }

        [TextArea] private string memo;
        
        public void AddBoth(object key, float modifyValue, float bufferValue)
        {
            AddModifier(key, modifyValue);
            AddBuffer(key, bufferValue);
        }
        public void RemoveBoth(object key)
        {
            RemoveModifier(key);
            RemoveBuffer(key);
        }

        public void AddModifier(object key, float modifyValue)
        {
            if (_modifiedValueDictionary.ContainsKey(key)) return;

            float prev = Value;
            
            _modifiedValueDictionary.Add(key, modifyValue);
            ModifyValue += modifyValue;
            
            TryInvokeValueChangeEvent(Value, prev);
        }

        public void AddBuffer(object key, float bufferValue)
        {
            if (_bufferValueDictionary.ContainsKey(key)) return;
            
            float prev = Value;
            
            _bufferValueDictionary.Add(key, bufferValue);
            BufferValue += bufferValue;
            
            TryInvokeValueChangeEvent(Value, prev);
        }

        public void RemoveModifier(object key)
        {
            if (_modifiedValueDictionary.ContainsKey(key))
            {
                float prev = Value;
                
                ModifyValue -= _modifiedValueDictionary[key];
                _modifiedValueDictionary.Remove(key);
                
                TryInvokeValueChangeEvent(Value, prev);
            }
        }
        public void RemoveBuffer(object key)
        {
            if (_bufferValueDictionary.ContainsKey(key))
            {
                float prev = Value;
                
                BufferValue -= _modifiedValueDictionary[key];
                _modifiedValueDictionary.Remove(key);
                
                TryInvokeValueChangeEvent(Value, prev);
            }
        }

        public void ClearAll()
        {
            float prev = Value;

            ModifyValue = 0;
            BufferValue = 1;
            _modifiedValueDictionary.Clear();
            _bufferValueDictionary.Clear();
            
            TryInvokeValueChangeEvent(Value, prev);
        }
        
        /// <summary>
        /// ValueChangedEvent를 실행하려 시도합니다. Value Change가 False라면 아무것도 하지 않습니다.
        /// </summary>
        /// <param name="currentValue">새로운 Value</param>
        /// <param name="prevValue">기존의 Value</param>
        private void TryInvokeValueChangeEvent(float currentValue, float prevValue)
        {
            if (Mathf.Approximately(prevValue, currentValue) == false)
            {
                OnValueChanged?.Invoke(this, currentValue, prevValue);
            }
        }

        public object Clone()
        {
            return Instantiate(this);
        }
    }
}