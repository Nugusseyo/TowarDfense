using System;
using TMPro;
using UnityEngine;

namespace _Scripts.Managers.LifeManager
{
    public class LifeViewer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lifeText;
        private void Start()
        {
            LifeManager.Instance.OnLifeChanged += HandleLifeChanged;
            lifeText.text = LifeManager.Instance.CurrentLife.ToString();
        }

        private void OnDestroy()
        {
            if(LifeManager.Instance != null)
                LifeManager.Instance.OnLifeChanged -= HandleLifeChanged;
        }

        private void HandleLifeChanged(int life)
        {
            lifeText.text = life.ToString();
        }
    }
}
