using System.Collections;
using System;
using System.Diagnostics;
using _Script.ScriptableObject.Event;
using _Scripts.Agent;
using _Scripts.Agent.Player;
using _Scripts.Agent.Tower;
using _Scripts.Managers.InfoM;
using GameModules._Script.Agent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Debug = UnityEngine.Debug;

namespace _Scripts.UI
{
    public class AgentUIViewer : MonoBehaviour
    {
        [field: SerializeField] public EventChannelSO ViewUIEventChannelSO { get; private set; }

        #region Middle_Left UI
        [Header("Information")]
        [SerializeField] private GameObject uiParent;
        [SerializeField] private GameObject worldCanvas;
        
        [SerializeField] private Image portrait;
        [SerializeField] private TextMeshProUGUI agentName;
        [SerializeField] private TextMeshProUGUI agentPosition;
        
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI reloadText;
        
        [Header("Skills")]
        [SerializeField] private Image skillIcon;
        [SerializeField] private Image skillCooldownCover;
        [SerializeField] private TextMeshProUGUI skillDesc;
        [SerializeField] private TextMeshProUGUI skillName;
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
        
        private CanvasGroup _uiParentCG;
        private CanvasGroup _worldCanvasCG;

        private const string HEALER = "치유가";
        private const string TANKER = "선봉가";
        private const string SURPPORT = "지원가";
        private const string NONE = "적군";

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
            if(InfoManager.Instance != null)
                InfoManager.Instance.HideRangeDecal();

            ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentOnUI.Init(null)); //카메라 기본 위치로 되돌리기 위함.

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
                if (gameObject.activeSelf)
                {
                    if (_removeWaitCoroutine != null) StopCoroutine(_removeWaitCoroutine);
                    _removeWaitCoroutine = StartCoroutine(RemoveWaitSecond());
                }
                else
                    ClearCurrentAgent();
                
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
            }
            AgentUIDataSO data = agent.UIData;

            if (_curSkillModule == null || data == null || _curAttackModule == null)
            {
                Debug.LogError("해당 객체에게 IAgentSkillModule 또는 UIData 또는 IAgentAttackModule가 누락되었습니다.");
                return;
            }
            
            agentName.text = data.agentName;
            agentPosition.text = data.playerType switch
            {
                PlayerType.HEALER => HEALER,
                PlayerType.TANKER => TANKER,
                PlayerType.SURPPORT => SURPPORT,
                _ => NONE
            };
            portrait.sprite = data.portrait;
            
            costText.text = "코스트 " + data.cost;
            reloadText.text = agent.AgentStatusSO.SkillAttackCooldown + "초";
            
            skillIcon.sprite = data.skillIcon;
            skillDesc.text = data.skillDesc;
            skillName.text = data.skillName;
            _skillBtnImage.sprite = data.skillIcon;

            Vector3 fixPos = agent.transform.position;
            fixPos.y = worldCanvas.transform.position.y;
            worldCanvas.transform.position = fixPos;
            
            _uiParentCG.DOKill();
            _uiParentCG.alpha = 0f;
            uiParent.SetActive(true);
            _uiParentCG.DOFade(1f, 0.25f).SetEase(Ease.OutQuad);

            _worldCanvasCG.DOKill();
            _worldCanvasCG.alpha = 0f;
            
            if(agent is not Tower)
                worldCanvas.SetActive(true);
            else
                worldCanvas.gameObject.SetActive(false);
            
            _worldCanvasCG.DOFade(1f, 0.25f).SetEase(Ease.OutQuad);
        }

        private IEnumerator RemoveWaitSecond()
        {
            yield return new WaitForSeconds(0.1f);
            if (!_isActive)
            {
                _uiParentCG.DOKill();
                _worldCanvasCG.DOKill();
                
                _worldCanvasCG.DOFade(0f, 0.25f).SetEase(Ease.OutQuad);
                _uiParentCG.DOFade(0f, 0.25f).SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        uiParent.SetActive(false);
                        worldCanvas.SetActive(false);
                    });
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
                _uiParentCG.DOFade(1f, 0.5f).SetEase(Ease.OutQuad);
            }
            
            _worldCanvasCG.DOKill();
            worldCanvas.SetActive(false);

            if (evt.Agent == null) return;
            
            AgentUIDataSO data = evt.Agent.UIData;
            
            agentName.text = data.agentName;
            portrait.sprite = data.portrait;
            
            costText.text = "코스트 " + data.cost;
            reloadText.text = data.respawnTimer + "초";
            
            skillIcon.sprite = data.skillIcon;
            skillDesc.text = data.skillDesc;
            skillName.text = data.skillName;
        }
        
        private void ClearCurrentAgent()
        {
            _curSkillModule = null;
            _curAttackModule = null;
            _curOperator = null;
        }
    }
}