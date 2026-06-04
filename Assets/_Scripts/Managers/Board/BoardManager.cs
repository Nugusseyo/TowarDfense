using System.Collections.Generic;
using System.Linq;
using _Script.ScriptableObject.Event;
using _Script.Tools.Utility;
using _Scripts.Agent.Player;
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
            OperatorWrapper operatorWrapper = HoldOperatorListSO.GetOperator(index);
            if (operatorWrapper == null) return;

            // 💡 개선: 이미 배치 모드인 상태에서 다른 버튼을 눌렀을 때의 예외 처리 추가
            if (_isSpawning || _currentOperatorInfo != null)
            {
                if (_currentOperatorInfo?.operatorPrefab == operatorWrapper.operatorPrefab)
                {
                    CancelSpawning();
                    return;
                }
                
                ResetSpawningStateOnly(); // 💡 개선: 다른 캐릭터 버튼을 눌렀다면 이전 캐싱 데이터와 이벤트를 안전하게 선행 초기화
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
            
            ShowMountainEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(false));
            ShowGroundEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(false));
            
            if(operatorWrapper.isMountain)
                ShowMountainEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(true));
            else
                ShowGroundEventChannelSO.RaiseEvent(DecalEvents.DecalShow.Init(true));
            
            Debug.Log($"{operatorWrapper.operatorPrefab.name} 배치 모드 시작! 설치할 타일을 클릭하세요.");
            
            InputSO.OnLeftBtnClick -= HandlePlacement; // 💡 개선: 다른 버튼 누름으로 인한 델리게이트 중복 구독 현상 원천 차단
            InputSO.OnLeftBtnClick += HandlePlacement;
            
            ViewUIEventChannelSO.RaiseEvent(AgentEvents.AgentInfoUI.Init(_currentOperatorInfo.operatorPrefab, true));
        }

        private void HandlePlacement()
        {
            // 💡 핵심 개선: 마우스가 UI 요소 위에 떠 있다면 월드 클릭(레이캐스트 피킹) 연산을 생략하여 Vector.zero 소환 현상 방지
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

                CostManager.CostManager.Instance.Cost -= newOperator.UIData.cost;
                
                _isSpawning = false;
                InputSO.OnLeftBtnClick -= HandlePlacement;
            }
            else
            {
                return; // 💡 개선: 유효한 레이어 감지에 실패했을 경우 데이터가 튀는 것을 막고 그대로 리턴하여 재입력 대기
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

        // 💡 개선: 순수 내부 배치 상태 변수와 이벤트만 안전하게 분리 수거하는 공용 메서드
        private void ResetSpawningStateOnly()
        {
            InputSO.OnLeftBtnClick -= HandlePlacement;
            _isSpawning = false;
            _currentOperatorInfo = null;
        }

        // 💡 개선: 배치 모드를 도중에 탈출하거나 같은 버튼을 다시 눌러 취소할 때 사용하는 완전 복구 메서드
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
            }
        }
    }
}