using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class SoundSetter : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private TextMeshProUGUI valueTmp;
        
        [SerializeField] private string volumeParameterName = "SFX"; 
        
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponentInChildren<Slider>();

            if (_slider == null)
            {
                Debug.LogError($"[SoundSetter] {gameObject.name}에 Slider 컴포넌트가 없습니다!");
                return;
            }

            if (audioMixer == null)
            {
                Debug.LogError($"[SoundSetter] {gameObject.name}에 AudioMixer가 지정되지 않았습니다!");
                return;
            }
            
            _slider.minValue = 0.0001f; 
            _slider.maxValue = 1f;
            
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void Start()
        {
            if (audioMixer.GetFloat(volumeParameterName, out float currentDb))
            {
                // 데시벨(dB)을 다시 0~1 사이의 비율 값으로
                _slider.value = Mathf.Pow(10f, currentDb / 20f);
                UpdateValueText(_slider.value);
            }
        }

        private void OnSliderValueChanged(float value)
        {
            float dbVolume = Mathf.Log10(value) * 20f;
            
            audioMixer.SetFloat(volumeParameterName, dbVolume);
            UpdateValueText(value);
        }

        private void UpdateValueText(float value)
        {
            if (valueTmp != null)
            {
                int percentage = Mathf.RoundToInt(value * 100f);
                valueTmp.text = $"{percentage}";
            }
        }
    }
}
