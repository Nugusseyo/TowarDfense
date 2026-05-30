using System;
using System.Collections;
using _Script.ScriptableObject.Event;
using _Scripts.Agent;
using GameModules._Script.Agent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class AgentUIViewer : MonoBehaviour
    {
        [field: SerializeField] public EventChannelSO ViewUIEventChannelSO { get; private set; }

        #region Middle_Left UI
        [Header("Information")]
        [SerializeField] private GameObject uiParent;
        [SerializeField] private GameObject worldCanvas;
        
        [SerializeField] private Image positionIcon;
        [SerializeField] private TextMeshProUGUI agentName;
        
        [Header("Skills")]
        [SerializeField] private Image skillIcon;
        [SerializeField] private Image skillCooldownCover;
        [SerializeField] private TextMeshProUGUI skillDesc;
        #endregion
        
        #region Middle_Center UI
        
        [Header("CenterUI")]
        [SerializeField] private Button skillBtn;                   //  이미지는 Skill Icon과 동일.
        [SerializeField] private TextMeshProUGUI skillPercent;
        [SerializeField] private Image percentBG;
        [SerializeField] private Gradient percentGradient;
        private Image _skillBtnImage;                                //  GetCompo로 받아옴.
        
        #endregion

        private IAgentSkillModule _curSkillModule;
        private IAgentAttackModule _curAttackModule;

        private void Awake()
        {
            ViewUIEventChannelSO.AddListener<AgentOnUI>(HandleAgentSelect);
            skillBtn.onClick.AddListener(HandleSkillButtonClick);
            _skillBtnImage = skillBtn.GetComponent<Image>();
        }

        private void Start()
        {
            uiParent.SetActive(false);
            worldCanvas.SetActive(false);
        }

        private void HandleSkillButtonClick()
        {
            _curAttackModule.UseSkill();
            uiParent.SetActive(false);
            worldCanvas.SetActive(false);
        }

        private void OnDestroy()
        {
            ViewUIEventChannelSO.RemoveListener<AgentOnUI>(HandleAgentSelect);
            if(skillBtn != null)
                skillBtn.onClick.RemoveListener(HandleSkillButtonClick);
        }

        private void HandleAgentSelect(AgentOnUI evt)
        {
            if (evt.NextAgent == null)
            {
                StartCoroutine(RemoveWaitSecond());
                //Camera Moving 추가하기
                return;
            }
            StopCoroutine(RemoveWaitSecond());
            
            Agent.Agent agent = evt.NextAgent;
            _curSkillModule = agent.GetModule<IAgentSkillModule>();
            _curAttackModule = agent.GetModule<IAgentAttackModule>();
            AgentUIDataSO data = agent.UIData;

            if (_curSkillModule == null || data == null || _curAttackModule == null)
            {
                Debug.LogError("해당 객체에게 IAgentSkillModule 또는 UIData 또는 IAgentAttackModule가 누락되었습니다.");
                return;
            }
            
            uiParent.SetActive(true);
            worldCanvas.SetActive(true);

            positionIcon.sprite = data.positionTypeIcon;
            agentName.text = data.agentName;
            
            skillIcon.sprite = data.skillIcon;
            skillDesc.text = data.skillDesc;
            _skillBtnImage.sprite = data.skillIcon;

            Vector3 fixPos = agent.transform.position;
            fixPos.y = worldCanvas.transform.position.y;
            worldCanvas.transform.position = fixPos;

        }

        private IEnumerator RemoveWaitSecond()
        {
            yield return new WaitForSeconds(0.1f);
            uiParent.SetActive(false);
            worldCanvas.SetActive(false);
        }

        private void Update()
        {
            if(_curSkillModule == null) return;

            float fPercent = _curSkillModule.GetCooldownNormal;
            int percent = Mathf.RoundToInt(fPercent * 100);

            if (float.IsNaN(fPercent))
            {
                skillPercent.text = "사용 불가";
                percentBG.color = percentGradient.Evaluate(1);
                skillCooldownCover.fillAmount = 1;
                skillBtn.enabled = false;
                return;
            }

            if (percent >= 100)
            {
                skillPercent.text = "Ready!";
                skillCooldownCover.fillAmount = 0;
                skillBtn.enabled = true;
            }
            else
            {
                skillPercent.text = percent + "/100";
                skillCooldownCover.fillAmount = fPercent;
                skillBtn.enabled = false;
            }
            
            percentBG.color = percentGradient.Evaluate(fPercent);
        }
    }
}
