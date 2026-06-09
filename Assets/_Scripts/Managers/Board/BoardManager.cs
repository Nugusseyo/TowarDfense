using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Script.ScriptableObject.Event;
using _Script.Tools.Utility;
using _Scripts.Agent.Player;
using _Scripts.Managers.InfoM;
using _Scripts.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Managers.Board
{
    public class BoardManager : MonoSingleton<BoardManager>
    {
        [field: SerializeField] public InputSO InputSO { get; private set; }
        [field: SerializeField] public HoldOperListSO HoldOperatorListSO { get; private set; }
        [field: SerializeField] public EventChannelSO ShowGroundEventChannelSO { get; private set; }
        [field: SerializeField] public EventChannelSO ShowMountainEventChannelSO { get; private set; }
        [field: SerializeField] public EventChannelSO ViewUIEventChannelSO { get; private set; }
        [field: SerializeField] public EventChannelSO CameraEventChannelSO { get; private set; }
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask mountainLayer;

        private Dictionary<Vector2Int, AbstractOperator> _operators = new Dictionary<Vector2Int, AbstractOperator>();
        
        private Dictionary<AbstractOperator, int> _spawnedOperatorIndex = new Dictionary<AbstractOperator, int>();
        
        private int _curOperatorIndex = -1;

        public Grid Grid { get; private set; }

        private bool _isSpawning = false;
        private OperatorWrapper _currentOperatorInfo = null;
        
        private bool _isPlacementRequested = false;

        [field: SerializeField] public int MaxOperatorCount = 8;
        public event Action<int> OnOperatorCountChanged; 
        private int _currentOperatorCount;
        public int CurrentOperatorCount
        {
            get => _currentOperatorCount;
            set
            {
                _currentOperatorCount = Mathf.Clamp(value, 0, MaxOperatorCount);
                OnOperatorCountChanged?.Invoke(_currentOperatorCount);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            Grid = GetComponent<Grid>();
            if (InputSO != null)
                InputSO.ChangeInput(true);
        }

        private void Start()
        {
            CurrentOperatorCount = MaxOperatorCount;
        }
        
        private void Update()
        {
            if (_isPlacementRequested) //이거 관련한 내용은 InfoManager 참고
            {
                _isPlacementRequested = false;
                PlacementLogic();
            }
        }

        [ContextMenu("Spawn 0")]
        public void SpawnOne()
        {
            SpawnOperator(0);
        }
        
        public void SpawnOperator(int index)
        {
            if(CurrentOperatorCount == 0)
            {
                Debug.LogWarning("Operator의 수가 없습니다.");
                return;
            }
            OperatorWrapper operatorWrapper = HoldOperatorListSO.GetOperator(index);
            if (operatorWrapper == null) return;
            
            if (_isSpawning || _currentOperatorInfo != null)
            {
                if (_currentOperatorInfo?.operatorPrefab == operatorWrapper.operatorPrefab)
                {
                    CancelSpawning();
                    return;
                }
                
                ResetSpawningStateOnly();
            }
            else
            {
                InputSO.ChangeInput(false);
            }
            
            if (_operators.Any(x => x.Value.UIData == operatorWrapper.operatorPrefab.UIData))
            {
                Debug.LogWarning("이미 소환된 오퍼레이터입니다!");
                InputSO.ChangeInput(true);
                return;
            }

            if (operatorWrapper.operatorPrefab.UIData.cost > CostManager.CostManager.Instance.Cost)
            {
                Debug.LogWarning("소환하려는 대상의 Cost가 현재 Cost보다 높은데 소환이 허락되었습니다.");
                InputSO.ChangeInput(true);
                return;
            }
            
            _currentOperatorInfo = operatorWrapper;
            _isSpawning = true;
            
            _curOperatorIndex = index;
            
            ShowMountainEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(false));
            ShowGroundEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(false));
            
            if(operatorWrapper.isMountain)
                ShowMountainEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(true));
            else
                ShowGroundEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(true));
            
            Debug.Log($"{operatorWrapper.operatorPrefab.name} 배치 모드 시작! 설치할 타일을 클릭하세요.");
            
            InputSO.OnLeftBtnClick -= HandlePlacement;
            InputSO.OnLeftBtnClick += HandlePlacement;
            
            ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentInfoUI.Init(_currentOperatorInfo.operatorPrefab, true));
        }
        
        private void HandlePlacement()
        {
            _isPlacementRequested = true;
        }
        
        private void PlacementLogic()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (_currentOperatorInfo == null)
            {
                CancelSpawning();
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
                
                Vector3 spawnWorldPos = Grid.GetCellCenterWorld(cellPos);
                int yPos = isMountain ? 3 : 1;
                spawnWorldPos.y = yPos;
                
                AbstractOperator newOperator = Instantiate(_currentOperatorInfo.operatorPrefab, spawnWorldPos, Quaternion.identity);
                newOperator.gameObject.name = newOperator.UIData.agentName;
                
                _operators.Add(gridPos, newOperator);
                Debug.Log($"[{gridPos}] 칸에 {newOperator.name} 배치 완료!");
                
                if (_curOperatorIndex != -1)
                {
                    if (OperatorInfoInjector.Instance != null)
                        OperatorInfoInjector.Instance.ActiveButton(_curOperatorIndex, false);
                        
                    _spawnedOperatorIndex.Add(newOperator, _curOperatorIndex);
                    _curOperatorIndex = -1;
                }

                CurrentOperatorCount--;
                CostManager.CostManager.Instance.Cost -= newOperator.UIData.cost;
                
                _isSpawning = false;
                InputSO.OnLeftBtnClick -= HandlePlacement;
            }
            else
            {
                return;
            }
            
            if(_currentOperatorInfo.isMountain)
                ShowMountainEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(_isSpawning));
            else
                ShowGroundEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(_isSpawning));

            if (!_isSpawning)
            {
                _currentOperatorInfo = null;
                ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentInfoUI.Init(null, false));
            }
            InputSO.ChangeInput(!_isSpawning);
        }
        
        private void ResetSpawningStateOnly()
        {
            InputSO.OnLeftBtnClick -= HandlePlacement;
            _isSpawning = false;
            _currentOperatorInfo = null;
            _isPlacementRequested = false;
            _curOperatorIndex = -1;
        }
        
        private void CancelSpawning()
        {
            ResetSpawningStateOnly();
            ShowMountainEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(false));
            ShowGroundEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(false));
            ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentInfoUI.Init(null, false));
            InputSO.ChangeInput(true);
        }

        public void RemoveDictionary(AbstractOperator abstractOperator)
        {
            if (_operators.ContainsValue(abstractOperator))
            {
                Debug.Log("죽은 Operator가 Dictionary에 포함되어있음. 제거 시작");
                KeyValuePair<Vector2Int, AbstractOperator> pair = _operators.FirstOrDefault(x => x.Value ==  abstractOperator);
                _operators.Remove(pair.Key);


                
                if (_spawnedOperatorIndex.TryGetValue(abstractOperator, out int targetIndex))
                {
                    if (OperatorInfoInjector.Instance != null)
                        OperatorInfoInjector.Instance.ActiveButton(targetIndex, true);
                        
                    _spawnedOperatorIndex.Remove(abstractOperator);
                }
                
                CurrentOperatorCount++;
            }
        }
    }
}