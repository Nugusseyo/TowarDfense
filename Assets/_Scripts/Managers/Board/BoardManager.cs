using System.Collections.Generic;
using System.Linq;
using _Script.ScriptableObject.Event;
using _Script.Tools.Utility;
using _Scripts.Agent.Player;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Managers.Board
{
    public class BoardManager : MonoSingleton<BoardManager>
    {
        [field: SerializeField] public InputSO InputSO { get; private set; }
        [field: SerializeField] public HoldOperListSO HoldOperatorListSO { get; private set; }
        [field: SerializeField] public EventChannelSO ShowGroundEventChannelSO { get; private set; }
        [field: SerializeField] public EventChannelSO ShowMountainEventChannelSO { get; private set; }
        [field: SerializeField] public EventChannelSO ViewUIEventChannelSO { get; private set; }
        [SerializeField] private LayerMask groundLayer;

        [SerializeField] private LayerMask mountainLayer;

        private Dictionary<Vector2Int, AbstractOperator> _operators = new Dictionary<Vector2Int, AbstractOperator>();
        public Grid Grid { get; private set; }

        private bool _isSpawning = false;
        private OperatorWrapper _currentOperatorInfo = null;

        protected override void Awake()
        {
            base.Awake();
            Grid = GetComponent<Grid>();
            InputSO.ChangeInput(true);
        }

        [ContextMenu("Spawn 0")]
        public void SpawnOne()
        {
            SpawnOperator(0);
        }
        
        public void SpawnOperator(int index)
        {

            InputSO.OnLeftBtnClick = null;  //다른 오퍼레이터 눌러놓은거 초기화 시켜줘야함.
            //안그러면 중복 소환 됨;;
            
            OperatorWrapper operatorWrapper = HoldOperatorListSO.GetOperator(index);
            
            if (operatorWrapper == null) return;
            if (_currentOperatorInfo != null || _isSpawning)
            {
                if (_currentOperatorInfo.operatorPrefab != null)
                {
                    if (_currentOperatorInfo.operatorPrefab == operatorWrapper.operatorPrefab)
                    {
                        ShowMountainEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(false));
                        ShowGroundEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(false));
                        ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentInfoUI.Init(null, false));
                        InputSO.ChangeInput(true);
                        _isSpawning = false;
                        _currentOperatorInfo = null;
                        Debug.Log("같은 Operator누름 감지. 배치 취소합니다.");
                        return;
                    }
                }
            }
            
            
            if (_operators.Any(x => x.Value.UIData == operatorWrapper.operatorPrefab.UIData))
            {
                Debug.LogWarning("이미 소환된 오퍼레이터입니다!");
                return;
            }

            if (operatorWrapper.operatorPrefab.UIData.cost > CostManager.CostManager.Instance.Cost)
            {
                Debug.LogWarning("소환하려는 대상의 Cost가 현재 Cost보다 높은데 소환이 허락되었습니다.");
                return;
            }
            InputSO.ChangeInput(false);
            
            _currentOperatorInfo = operatorWrapper;
            _isSpawning = true;
            
            ShowMountainEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(false));
            ShowMountainEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(false));
            
            if(operatorWrapper.isMountain)
                ShowMountainEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(true));
            else
                ShowGroundEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(true));
            
            Debug.Log($"{operatorWrapper.operatorPrefab.name} 배치 모드 시작! 설치할 타일을 클릭하세요.");
            InputSO.OnLeftBtnClick += HandlePlacement;
            ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentInfoUI.Init(_currentOperatorInfo.operatorPrefab, true));
            //Agent.Agent agent = operatorWrapper.operatorPrefab.GetComponent<Agent.Agent>(); 
            //ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentOnUI.Init(agent));
            
            //Operator의 Info가 뜨게 하는 방법은 한 번 다르게 생각해보자.
        }
        
        /*
        private void Update()
        {
            // 배치 모드가 아니라면 마우스 클릭 감지를 아예 하지 않음 (성능 이득 야르!)
            if (!_isSpawning || _currentOperatorInfo == null) return;

            // 마우스 좌클릭을 했을 때
            if (Input.GetMouseButtonDown(0))
            {
                HandlePlacement();
            }
        }*/

        private void HandlePlacement()
        {
            if (_currentOperatorInfo == null)
            {
                InputSO.ChangeInput(true);
                _isSpawning = false;
                _currentOperatorInfo = null;
                InputSO.OnLeftBtnClick -= HandlePlacement;
                return;
            }
            
            bool isMountain = _currentOperatorInfo.isMountain;
            LayerMask targetLayer = isMountain ? mountainLayer : groundLayer;
            
            if (InputSO.GetMousePosByGameObject(out Vector3 pos, targetLayer))
            {
                Vector3Int cellPos = Grid.WorldToCell(pos);
                Vector2Int gridPos = new Vector2Int(cellPos.x, cellPos.z);
                
                if (_operators.ContainsKey(gridPos))
                {
                    Debug.LogWarning($"[{gridPos}] 칸에는 이미 오퍼레이터가 배치되어 있습니다!");
                    return;
                }
                
                //중앙 정렬
                Vector3 spawnWorldPos = Grid.GetCellCenterWorld(cellPos);
                
                int yPos = isMountain ? 3 : 1;
                spawnWorldPos.y = yPos;
                
                AbstractOperator newOperator = Instantiate(_currentOperatorInfo.operatorPrefab, spawnWorldPos, Quaternion.identity); //이거 풀링으로 고쳐야함.
                newOperator.gameObject.name = newOperator.UIData.agentName;
                
                _operators.Add(gridPos, newOperator);
                Debug.Log($"[{gridPos}] 칸에 {newOperator.name} 배치 완료!");

                CostManager.CostManager.Instance.Cost -= newOperator.UIData.cost;
                
                _isSpawning = false;
                InputSO.OnLeftBtnClick -= HandlePlacement;
            }
            else
            {
                Debug.Log("아무것도 보이지 않아...");
            }
            
            if(_currentOperatorInfo.isMountain)
                ShowMountainEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(_isSpawning));
            else
                ShowGroundEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(_isSpawning));

            if (!_isSpawning)
            {
                _currentOperatorInfo = null;
                ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentInfoUI.Init(null, false));
                //ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentOnUI.Init(null));
                //Operator Info 뜨게 하는거 다시 생각해보자 2
            }
            InputSO.ChangeInput(!_isSpawning);
        }

        public void RemoveDictionary(AbstractOperator abstractOperator)
        {
            if (_operators.ContainsValue(abstractOperator))
            {
                Debug.Log("죽은 Operator가 Dictionary에 포함되어있음. 제거 시작");
                //KeyValuePair : Dictionary가 뱉는 Pair return값.
                KeyValuePair<Vector2Int, AbstractOperator> pair = _operators.FirstOrDefault(x => x.Value ==  abstractOperator);
                _operators.Remove(pair.Key);
            }
        }
    }
}