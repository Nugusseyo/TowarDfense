using System.Collections.Generic;
using _Script.Tools.Utility;
using _Scripts.Agent.Player;
using UnityEngine;

namespace _Scripts.Managers.Board
{
    public class BoardManager : MonoSingleton<BoardManager>
    {
        [field: SerializeField] public InputSO InputSO { get; private set; }
        [field: SerializeField] public HoldOperListSO HoldOperatorListSO { get; private set; }
        [SerializeField] private LayerMask groundLayer;

        [SerializeField] private LayerMask mountainLayer;

        private Dictionary<Vector2Int, OperatorWrapper> _operators = new Dictionary<Vector2Int, OperatorWrapper>();
        public Grid Grid { get; private set; }

        private bool _isSpawning = false;
        private OperatorWrapper _currentOperatorInfo = null;

        private void Awake()
        {
            Grid = GetComponent<Grid>();
        }

        [ContextMenu("Spawn 0")]
        public void SpawnOne()
        {
            SpawnOperator(0);
        }

        // 1. UI 등에서 "몇 번 오퍼레이터 소환할래요" 하고 찔러주는 함수
        public void SpawnOperator(int index)
        {
            OperatorWrapper operatorWrapper = HoldOperatorListSO.GetOperator(index);
            
            // 프리팹 자체가 이미 필드에 배치된 녀석인지 체크 (프리팹 기준 검사라면 유효)
            if (_operators.ContainsValue(operatorWrapper))
            {
                Debug.LogWarning("이미 소환된 오퍼레이터입니다!");
                return;
            }

            // 설치 모드 돌입 및 정보 저장
            _currentOperatorInfo = operatorWrapper;
            _isSpawning = true;
            Debug.Log($"{operatorWrapper.operatorPrefab.name} 배치 모드 시작! 설치할 타일을 클릭하세요.");
        }

        private void Update()
        {
            // 배치 모드가 아니라면 마우스 클릭 감지를 아예 하지 않음 (성능 이득 야르!)
            if (!_isSpawning || _currentOperatorInfo == null) return;

            // 마우스 좌클릭을 했을 때
            if (Input.GetMouseButtonDown(0))
            {
                HandlePlacement();
            }
        }

        private void HandlePlacement()
        {
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
                
                _operators.Add(gridPos, _currentOperatorInfo);
                Debug.Log($"[{gridPos}] 칸에 {newOperator.name} 배치 완료!");
                
                _isSpawning = false;
                _currentOperatorInfo = null;
            }
            else
            {
                Debug.Log("아무것도 보이지 않아...");
            }
        }
    }
}