using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Managers.CostManager
{
    public class CostViewer : MonoBehaviour
    {
        [SerializeField] private Image bar;
        [SerializeField] private float lerpSpeed = 15f; 
        [SerializeField] private TextMeshProUGUI costText;
    
        private void Update()
        {
            if (CostManager.Instance == null) return;
            
            costText.text = CostManager.Instance.Cost.ToString();
            
            float currentCost = CostManager.Instance.GetNormalCost;
            if (currentCost < bar.fillAmount && bar.fillAmount > 0.8f)
            {
                bar.fillAmount = currentCost; 
            }
            else
            {
                bar.fillAmount = Mathf.Lerp(bar.fillAmount, currentCost, Time.deltaTime * lerpSpeed);
            }
        }
    }
}