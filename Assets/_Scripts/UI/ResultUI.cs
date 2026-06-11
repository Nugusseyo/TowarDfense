using System;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class ResultUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stageNumber;

        private void Awake()
        {
            stageNumber.text = "1-1";
        }
    }
}
