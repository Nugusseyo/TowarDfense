using System.Collections.Generic;
using _Script.ScriptableObject.Event;
using UnityEngine;

namespace _Scripts.Agent.Enemy.Citizens
{
    public class CitizenSpawner : MonoBehaviour, ICitizenSpawner
    {
        [Header("Prefab Settings")]
        [SerializeField] private GameObject citizenPrefab;
        
        [Header("WayPoint Settings")]
        [SerializeField] private List<CitizenWayPointData> wayPointsInput = new List<CitizenWayPointData>();

        private readonly List<CitizenWayPoint> _cachedWayPoints = new List<CitizenWayPoint>();

        private void Awake()
        {
            // 인스펙터용 직렬화 구조체를 실제 CitizenWayPoint 클래스로 변환 및 캐싱
            InitializeWayPoints();
        }

        public GameObject SummonCitizen(Agent citizen)
        {
            if (citizen is not AbstractCitizen)
            {
                Debug.LogError("해당 객체는 Citizen이 아닙니다!");
                return null;
            }
            citizenPrefab = citizen.gameObject;
            return SpawnCitizen();
        }

        [ContextMenu("Spawn Citizen")]
        private GameObject SpawnCitizen()
        {
            if (citizenPrefab == null)
            {
                Debug.LogError($"[{name}] Citizen 프리팹이 등록되지 않았습니다!");
                return null;
            }

            if (_cachedWayPoints.Count == 0)
            {
                Debug.LogWarning($"[{name}] 넘겨줄 웨이포인트가 비어있습니다. 인스펙터를 확인하세요.");
                return null;
            }
            Vector3 spawnPos = transform.position;
            GameObject citizen = Instantiate(citizenPrefab, spawnPos, Quaternion.identity);
            
            //Initialize
            if (citizen.TryGetComponent(out AbstractCitizen absCitizen))
            {
                ICitizenMover mover = absCitizen.GetModule<ICitizenMover>();
                mover.EnemyInitialize(_cachedWayPoints);
            }
            else
            {
                Debug.LogError($"{citizen.name} 프리팹에 Citizen 컴포넌트가 없습니다!");
            }
            return citizen;
        }

        private void InitializeWayPoints()
        {
            _cachedWayPoints.Clear();
            foreach (CitizenWayPointData data in wayPointsInput)
            {
                _cachedWayPoints.Add(new CitizenWayPoint
                {
                    TargetPos = data.targetTransform != null ? data.targetTransform.position : data.customPosition,
                    WaitSecond = data.waitSecond
                });
            }
        }

        #region Gizmos
        private void OnDrawGizmos()
        {
            if (wayPointsInput == null || wayPointsInput.Count == 0) return;

            Gizmos.color = Color.cyan;
            Vector3 previousPos = transform.position;
            previousPos.y = 4f;

            for (int i = 0; i < wayPointsInput.Count; i++)
            {
                Vector3 currentPos = wayPointsInput[i].targetTransform != null 
                    ? wayPointsInput[i].targetTransform.position 
                    : wayPointsInput[i].customPosition;

                currentPos.y = 4f;
                
                // 웨이포인트 지점에 구체 그리기
                Gizmos.DrawSphere(currentPos, 0.3f);
                
                // 경로선 그리기
                if (i == 0)
                {
                    Gizmos.color = Color.green; // 스포너 -> 첫 위치는 초록선
                    Gizmos.DrawLine(previousPos, currentPos);
                    Gizmos.color = Color.cyan;
                }
                else
                {
                    Gizmos.DrawLine(previousPos, currentPos);
                }

                previousPos = currentPos;
            }
        }
        #endregion
    }
    [System.Serializable]
    public struct CitizenWayPointData
    {
        [Tooltip("Transform을 배치하면 해당 오브젝트의 위치를 우선 사용합니다.")]
        public Transform targetTransform;
        [Tooltip("Transform이 비어있을 때만 이 좌표를 사용합니다.")]
        public Vector3 customPosition;
        public float waitSecond;
    }
}