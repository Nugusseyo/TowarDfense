using System;
using System.Linq;
using _Scripts.Agent.Enemy.Citizens;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class WaveViewer : MonoBehaviour
    {
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
        }
    }
}
