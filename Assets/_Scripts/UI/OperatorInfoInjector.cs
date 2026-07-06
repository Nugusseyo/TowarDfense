using System.Collections.Generic;
using _Script.Tools.Utility;
using _Scripts.Managers.Board;
using _Scripts.Managers.InfoM;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace _Scripts.UI
{
    public class OperatorInfoInjector : MonoSingleton<OperatorInfoInjector>
    {
        [field: SerializeField] public HoldOperListSO HoldOperator { get; private set; }
        public readonly Dictionary<int, CharacterButton> Buttons = new Dictionary<int, CharacterButton>(); //index , charBtn
        public GameObject buttonPrefab;
        public Transform buttonParent;

        protected override void Awake()
        {
            base.Awake();
            if (buttonPrefab == null || buttonParent == null)
            {
                Debug.LogError("오퍼레이터 버튼들 준비가 아직 안된 것 같네요.");
                return;
            }

            if (MapDataHolder.Instance != null)
                HoldOperator = MapDataHolder.Instance.HoldData.HoldOperatorList;
            
            for (int i = 0; i < HoldOperator.OperatorCount; ++i)
            {
                int index = i;
                OperatorWrapper wrapper = HoldOperator.GetOperator(index);
                AgentUIDataSO data = wrapper.operatorPrefab.UIData;
                GameObject button = Instantiate(buttonPrefab, buttonParent);
                Button buttonScript = button.GetComponent<Button>();
                CharacterButton charBtn = button.GetComponent<CharacterButton>();
                charBtn.Portrait.sprite = data.portrait;
                charBtn.TopText.text = data.cost.ToString();

                Buttons.TryAdd(index, charBtn); //Try Add : 해당 Key가 없으면 할당.
                
                buttonScript.onClick.AddListener(() => BoardManager.Instance.SpawnOperator(index));
            }
        }

        public void ActiveButton(int index, bool isActive)
        {
            if (!Buttons.TryGetValue(index, out CharacterButton charBtn)) return;

            charBtn.gameObject.SetActive(isActive);
            charBtn.CooldownCharacter(HoldOperator.GetOperator(index).operatorPrefab.AgentStatusSO.RespawnDelay);
        }
    }
}
