using System;
using System.Collections.Generic;
using _Script.Tools.Utility;
using _Scripts.Agent.Player;
using _Scripts.Managers.Board;
using UnityEngine;

namespace _Scripts.Managers
{
    // ⚠️ 딕셔너리 Key는 정밀도 버그 방지를 위해 Vector2Int로 고정!
    public class BoardManager : MonoSingleton<BoardManager>
    {
        [field: SerializeField] public HoldOperListSO HoldOperatorListSO { get; private set; }
        [SerializeField] private LayerMask groundLayer;

        private Dictionary<Vector2Int, AbstractOperator> _operators = new Dictionary<Vector2Int, AbstractOperator>();
        public Grid Grid { get; private set; }

        private bool _isSpawning = false;
        private AbstractOperator _pendingOperatorPrefab = null;

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
            AbstractOperator operatorPrefab = HoldOperatorListSO.GetOperator(index);
            
            // 프리팹 자체가 이미 필드에 배치된 녀석인지 체크 (프리팹 기준 검사라면 유효)
            if (_operators.ContainsValue(operatorPrefab))
            {
                Debug.LogWarning("이미 소환된 오퍼레이터입니다!");
                return;
            }

            // 설치 모드 돌입 및 정보 저장
            _pendingOperatorPrefab = operatorPrefab;
            _isSpawning = true;
            Debug.Log($"{operatorPrefab.name} 배치 모드 시작! 설치할 타일을 클릭하세요.");
        }

        private void Update()
        {
            // 배치 모드가 아니라면 마우스 클릭 감지를 아예 하지 않음 (성능 이득 야르!)
            if (!_isSpawning || _pendingOperatorPrefab == null) return;

            // 마우스 좌클릭을 했을 때
            if (Input.GetMouseButtonDown(0))
            {
                HandlePlacement();
            }
        }

        private void HandlePlacement()
        {
            // 2. 마우스 위치에서 카메라 레이저 발사 (3D 레이캐스트)
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                // 3. 월드 좌표를 그리드의 3D 셀 좌표(Vector3Int)로 변환
                Vector3Int cellPos = Grid.WorldToCell(hit.point);
                
                // 4. 3D 그리드(XZ 평면)를 2D 맵 좌표(Vector2Int)로 압축 가공
                Vector2Int gridPos = new Vector2Int(cellPos.x, cellPos.z); 

                // 5. [핵심 조건문] 이미 해당 격자 자리에 누군가 있다면? -> 그냥 무시하기!
                if (_operators.ContainsKey(gridPos))
                {
                    Debug.LogWarning($"[{gridPos}] 칸에는 이미 오퍼레이터가 배치되어 있습니다!");
                    return; 
                }

                // 6. 빈 공간이라면? 실제 인스턴스 생성 및 배치 진행!
                // 타일의 정중앙 월드 좌표를 계산해와서 정렬 배치합니다.
                Vector3 spawnWorldPos = Grid.GetCellCenterWorld(cellPos);
                spawnWorldPos.y = 4;
                
                AbstractOperator newOperator = Instantiate(_pendingOperatorPrefab, spawnWorldPos, Quaternion.identity);
                
                // 7. 관리용 딕셔너리에 '그리드 주소'와 '생성된 녀석'을 짝지어 등록
                _operators.Add(gridPos, newOperator);
                
                Debug.Log($"[{gridPos}] 칸에 {newOperator.name} 배치 완료!");

                // 8. 설치가 끝났으니 배치 모드 초기화 및 해제
                _isSpawning = false;
                _pendingOperatorPrefab = null;
            }
            else
            {
                Debug.Log("아무것도 보이지 않아...");
            }
        }
    }
}