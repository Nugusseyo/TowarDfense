using System;
using System.Collections;
using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using _Scripts.Agent.Tower;
using _Scripts.Managers.Board;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tutorialText;
        [SerializeField] private GameObject interactiveBlocker;
        public List<GameObject> highlights;
        private int _index = 0;

        [SerializeField] private EventChannelSO infoEventChannel;
        [SerializeField] private EventChannelSO gameEventChannel;
        [SerializeField] private Button skillBtn;
        
        private readonly WaitForSeconds _waitSecond = new  WaitForSeconds(3f);
        private bool _trigger = false;

        [SerializeField] private GameObject crossBow;
        [SerializeField] private GameObject opHealer;
        [SerializeField] private GameObject magician;
        [SerializeField] private GameObject bowwow;

        private void Start()
        {
            interactiveBlocker.SetActive(true);
            StartCoroutine(TutorialStart());
        }

        private IEnumerator TutorialStart()
        {
            yield return new WaitUntil(() => Time.timeScale >= 1);
            
            tutorialText.text = "튜토리얼에 오신 것을 환영합니다.";
            yield return _waitSecond;
            
            tutorialText.text = "기본적인 게임의 기능을 설명해드리겠습니다.";
            yield return _waitSecond;
            
            tutorialText.text = "우측 하단의 코스트를 이용해 요원을 소환할 수 있습니다.";
            HighLightObject();
            yield return _waitSecond;
            
            tutorialText.text = "요원 상단의 숫자는 코스트 비용입니다";
            HighLightObject();
            yield return _waitSecond;
            HighLightObject();
            
            tutorialText.text = "요원을 클릭해보세요.";
            interactiveBlocker.SetActive(false);
            infoEventChannel.AddListener<AgentInfoUI>(HandleAgentInfoUI);
            yield return new WaitUntil(() => _trigger);
            ResetHighLight();
            
            _trigger = false;
            tutorialText.text = "하단 바에서 요원을 클릭하거나, 배치된 요원을 클릭하면\n요원에 대한 정보를 얻을 수 있습니다.";
            yield return new WaitForSeconds(5f);
            
            tutorialText.text = "필드의 초록색 칸에 요원을 배치해보세요.";
            BoardManager.Instance.OnOperatorCountChanged += HandleOperatorChange;
            if(BoardManager.Instance.CurrentOperatorCount == 0) HandleOperatorChange(0);
            yield return new WaitUntil(() => _trigger);
            
            _trigger = false;
            tutorialText.text = "게임의 목표는 요원을 배치해 달걀을 지키는 것입니다.";
            yield return new WaitForSeconds(4f);

            tutorialText.text = "요원 체력 아래의 파란색 바는 스킬 게이지입니다.";
            yield return _waitSecond;

            tutorialText.text = "필드의 요원을 클릭하고, 요원 옆의 검은색 스킬 아이콘을 눌러 스킬을 발동하세요.";
            skillBtn.onClick.AddListener(HandleSkillBtnClick);
            yield return new WaitUntil(() => _trigger);
            _trigger = false;
            
            tutorialText.text = "스킬의 자세한 내용은 좌측에 나오는 요원 정보란에 있습니다.";
            yield return _waitSecond;
            
            tutorialText.text = "요원을 클릭하고, 검은색 스킬 아이콘 위의 쓰레기통 모양을 클릭하세요.";
            BoardManager.Instance.OnOperatorCountChanged += HandleOperatorRemove;
            if(BoardManager.Instance.CurrentOperatorCount == 1) HandleOperatorChange(1);
            yield return new WaitUntil(() => _trigger);
            
            tutorialText.text = "최대 요원의 수가 가득 찼을 때, 필요 없는 요원을 지우시면 됩니다.";
            yield return _waitSecond;

            tutorialText.text = "노란색 칸에는 타워(적)이 나와 달걀을 공격하게 됩니다.";
            yield return _waitSecond;
            
            Tower tower = Instantiate(crossBow, new Vector3(5, 3, 1), Quaternion.identity).GetComponent<Tower>();
            yield return _waitSecond;
            tutorialText.text = "타워는 웨이브가 지날수록 점점 강해집니다.";
            yield return _waitSecond;
            
            tutorialText.text = "\"진화체\" 요원을 소환해 타워를 공격하세요.";
            Instantiate(bowwow, new Vector3(5, 3, 5), Quaternion.identity);
            yield return new WaitForSeconds(3f);
            tutorialText.text = "\"의무병\" 요원을 소환해 요원을 회복하세요.";
            Instantiate(opHealer, new Vector3(1, 3, -3), Quaternion.identity);
            yield return new WaitForSeconds(4f);
            tutorialText.text = "\"연금술사\" 요원으로 달걀을 회복할 수 있습니다.";
            Instantiate(magician, new Vector3(5, 3, -1), Quaternion.identity);
            yield return new WaitForSeconds(4f);
            tutorialText.text = "\"연금술사\" 요원과 \"의무병\" 요원의 역할을 혼동하지 말아주시길 바랍니다.";
            yield return new WaitForSeconds(5f);
            
            tutorialText.text = "행운을 빕니다.";
            gameEventChannel.RaiseEvent(InGameEvents.GameEndEvent.Init(false));
        }

        private void HandleOperatorRemove(int evt)
        {
            if (evt == 0) return;
            _trigger = true;
            BoardManager.Instance.OnOperatorCountChanged -= HandleOperatorRemove;
        }

        private void HandleSkillBtnClick()
        {
            _trigger = true;
            skillBtn.onClick.RemoveListener(HandleSkillBtnClick);
        }

        private void HandleOperatorChange(int evt)
        {
            if (evt != 0) return;
            _trigger = true;
            BoardManager.Instance.OnOperatorCountChanged -= HandleOperatorChange;
        }

        private void HandleAgentInfoUI(AgentInfoUI evt)
        {
            if (evt.Agent == null) return;
            
            _trigger = true;
            infoEventChannel.RemoveListener<AgentInfoUI>(HandleAgentInfoUI);
        }

        private void HighLightObject()
        {
            if (_index == 0)
            {
                highlights[_index].SetActive(true);
                _index++;
                return;
            }
            highlights[_index - 1].SetActive(false);
            highlights[_index].SetActive(true);
            _index++;
        }
        
        private void ResetHighLight()
        {
            foreach (GameObject highlight in highlights)
            {
                highlight.SetActive(false);
            }
        }
    }
}
