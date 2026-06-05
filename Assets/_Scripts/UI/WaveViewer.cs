using System;
using System.Collections;
using System.Linq;
using _Script.ScriptableObject.Event;
using _Scripts.Agent.Enemy.Citizens;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class WaveViewer : MonoBehaviour
    {
        [field: SerializeField] public EventChannelSO GameEventChannel;
        [SerializeField] private TextMeshProUGUI waveText;
        private int _maxWave = 0;

        private void Start()
        {
            CreatureService.Instance.OnWaveChanged += HandleWaveChanged;
            _maxWave = CreatureService.Instance.creatureListSO.CreatureInfos.Max(x => x.wave);
            waveText.text = $"0 / {_maxWave}";
        }

        private void OnDestroy()
        {
            CreatureService.Instance.OnWaveChanged -= HandleWaveChanged;
        }
        
        private void HandleWaveChanged(int wave)
        {
            waveText.text = $"{Mathf.Clamp(wave, 0, _maxWave)} / {_maxWave}";

            if (_maxWave < wave)
            {
                GameEventChannel.RaiseEvent(InGameEvents.GameEndEvent.Init(false));
            }
        }
    }
}
