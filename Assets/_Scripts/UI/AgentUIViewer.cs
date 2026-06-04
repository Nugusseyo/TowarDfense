using System.Collections;
using System;
using _Script.ScriptableObject.Event;
using _Scripts.Agent;
using _Scripts.Agent.Player;
using GameModules._Script.Agent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // 💡 DOTween 네임스페이스 추가

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
        [SerializeField] private Button skillBtn;                   
        [SerializeField] private Button removeBtn;
        [SerializeField] private TextMeshProUGUI skillPercent;
        [SerializeField] private Image percentBG;
        [SerializeField] private Gradient percentGradient;
        private Image _skillBtnImage;                                
        
        #endregion

        private IAgentSkillModule _curSkillModule;
        private IAgentAttackModule _curAttackModule;
        private AbstractOperator _curOperator;

        private bool _isActive;
        private Coroutine _removeWaitCoroutine;

        // 💡 투명도 제어를 위한 CanvasGroup 변수
        private CanvasGroup _uiParentCG;
        private CanvasGroup _worldCanvasCG;

        private void Awake()
        {
            ViewUIEventChannelSO.AddListener<AgentOnUI>(HandleAgentSelect);
            ViewUIEventChannelSO.AddListener<AgentInfoUI>(HandleAgentInfoView);
            skillBtn.onClick.AddListener(HandleSkillButtonClick);
            removeBtn.onClick.AddListener(HandleRemoveButtonClick);
            _skillBtnImage = skillBtn.GetComponent<Image>();

            removeBtn.gameObject.SetActive(false);

            _uiParentCG = uiParent.GetComponent<CanvasGroup>();
            _worldCanvasCG = worldCanvas.GetComponent<CanvasGroup>();
        }

        private void HandleRemoveButtonClick()
        {
            if (_curOperator != null)
            {
                _curOperator.OnDeath();
            }
            
            _uiParentCG.DOKill();
            _worldCanvasCG.DOKill();
            uiParent.SetActive(false);
            worldCanvas.SetActive(false);

            ClearCurrentAgent(); 
            removeBtn.gameObject.SetActive(false);
        }

        private void Start()
        {
            uiParent.SetActive(false);
            worldCanvas.SetActive(false);
        }

        private void HandleSkillButtonClick()
        {
            _curAttackModule.UseSkill();
            
            _uiParentCG.DOKill();
            _worldCanvasCG.DOKill();
            uiParent.SetActive(false);
            worldCanvas.SetActive(false);

            ClearCurrentAgent();
        }

        private void OnDestroy()
        {
            ViewUIEventChannelSO.RemoveListener<AgentOnUI>(HandleAgentSelect);
            ViewUIEventChannelSO.RemoveListener<AgentInfoUI>(HandleAgentInfoView);
            if(skillBtn != null)
                skillBtn.onClick.RemoveListener(HandleSkillButtonClick);
            if(removeBtn != null)
                removeBtn.onClick.RemoveListener(HandleRemoveButtonClick);

            
            _uiParentCG.DOKill();
            _worldCanvasCG.DOKill();
        }

        private void HandleAgentSelect(AgentOnUI evt)
        {
            if (evt.NextAgent == null)
            {
                if (_removeWaitCoroutine != null) StopCoroutine(_removeWaitCoroutine);
                _removeWaitCoroutine = StartCoroutine(RemoveWaitSecond());
                return;
            }
            
            if (_removeWaitCoroutine != null) 
            {
                StopCoroutine(_removeWaitCoroutine);
                _removeWaitCoroutine = null;
            }
            
            Agent.Agent agent = evt.NextAgent;
            _curSkillModule = agent.GetModule<IAgentSkillModule>();
            _curAttackModule = agent.GetModule<IAgentAttackModule>();
            if (agent is AbstractOperator abstractOperator)
            {
                removeBtn.gameObject.SetActive(true);
                _curOperator = abstractOperator;
                if(abstractOperator == null)
                    Debug.LogError("AbstractOperator가 아닙니다!");
            }
            else
            {
                Debug.LogWarning("AbstractOperator가 아닙니다!");
            }
            AgentUIDataSO data = agent.UIData;

            if (_curSkillModule == null || data == null || _curAttackModule == null)
            {
                Debug.LogError("해당 객체에게 IAgentSkillModule 또는 UIData 또는 IAgentAttackModule가 누락되었습니다.");
                return;
            }
            
            positionIcon.sprite = data.positionTypeIcon;
            agentName.text = data.agentName;
            
            skillIcon.sprite = data.skillIcon;
            skillDesc.text = data.skillDesc;
            _skillBtnImage.sprite = data.skillIcon;

            Vector3 fixPos = agent.transform.position;
            fixPos.y = worldCanvas.transform.position.y;
            worldCanvas.transform.position = fixPos;
            
            _uiParentCG.DOKill();
            _uiParentCG.alpha = 0f;
            uiParent.SetActive(true);
            _uiParentCG.DOFade(1f, 0.1f).SetEase(Ease.OutQuad);

            _worldCanvasCG.DOKill();
            _worldCanvasCG.alpha = 0f;
            worldCanvas.SetActive(true);
            _worldCanvasCG.DOFade(1f, 0.1f).SetEase(Ease.OutQuad);
        }

        private IEnumerator RemoveWaitSecond()
        {
            yield return new WaitForSeconds(0.1f);
            if (!_isActive)
            {
                _uiParentCG.DOKill();
                _worldCanvasCG.DOKill();
                uiParent.SetActive(false);
                worldCanvas.SetActive(false);

                ClearCurrentAgent();
            }
            _removeWaitCoroutine = null;
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
        
        private void HandleAgentInfoView(AgentInfoUI evt)
        {
            _isActive = evt.IsActive;
            
            _uiParentCG.DOKill();
            uiParent.SetActive(_isActive);

            if (_isActive)
            {
                _uiParentCG.alpha = 0f;
                _uiParentCG.DOFade(1f, 0.1f).SetEase(Ease.OutQuad);
            }
            
            _worldCanvasCG.DOKill();
            worldCanvas.SetActive(false);

            if (evt.Agent == null) return;
            
            AgentUIDataSO data = evt.Agent.UIData;
            
            positionIcon.sprite = data.positionTypeIcon;
            agentName.text = data.agentName;
            
            skillIcon.sprite = data.skillIcon;
            skillDesc.text = data.skillDesc;
        }
        
        private void ClearCurrentAgent()
        {
            _curSkillModule = null;
            _curAttackModule = null;
            _curOperator = null;
        }
    }
}