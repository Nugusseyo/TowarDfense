using System;
using _Scripts.Managers.InfoM;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class ResultUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stageNumber;

        private void Awake()
        {
            if(MapDataHolder.Instance != null)
                stageNumber.text = $"{MapDataHolder.Instance.HoldData.BigStageNumber}-{MapDataHolder.Instance.HoldData.SmallStageNumber}";
        }
    }
}
