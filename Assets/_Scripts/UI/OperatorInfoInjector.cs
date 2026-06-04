using _Scripts.Managers.Board;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace _Scripts.UI
{
    public class OperatorInfoInjector : MonoBehaviour
    {
        [field: SerializeField] public HoldOperListSO HoldOperator { get; private set; }
        public GameObject buttonPrefab;
        public Transform buttonParent;

        private void Awake()
        {
            if (buttonPrefab == null || buttonParent == null)
            {
                Debug.LogError("오퍼레이터 버튼들 준비가 아직 안된 것 같네요.");
                return;
            }
            for (int i = 0; i < HoldOperator.OperatorCount; ++i)
            {
                int index = i;
                OperatorWrapper wrapper = HoldOperator.GetOperator(index);
                AgentUIDataSO data = wrapper.operatorPrefab.UIData;
                GameObject button = Instantiate(buttonPrefab, buttonParent);
                Button buttonScript = button.GetComponent<Button>();
                Image image = button.GetComponent<Image>();
                TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
                image.sprite = data.portrait;
                text.text = data.cost.ToString();
                
                buttonScript.onClick.AddListener(() => BoardManager.Instance.SpawnOperator(index));
            }
        }
    }
}
