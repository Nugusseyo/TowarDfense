using UnityEditor;
using UnityEngine;

namespace _Scripts.CustomEditor
{
    public class CustomGrid : MonoBehaviour
    {
        [Header("그리드 설정")]
        public Vector3 gridSize = new Vector3(10f, 10f, 10f); // 전체 그리드 부피
        public float spacing = 1f;                           // 그리드 칸 크기
        public Color gridColor = Color.cyan;                 // 그리드 선 색상

        [Header("가시성 제어")]
        public bool showTop = false;    // 상단 그리드 표시 여부 (기본값 False)
        public bool showBottom = false; // 하단 그리드 표시 여부 (기본값 False)
        public bool showSides = true;   // 측면(앞, 뒤, 좌, 우) 그리드 표시 여부
    }

#if UNITY_EDITOR
    // 인스펙터 및 씬 뷰를 확장하는 커스텀 에디터 클래스
    [UnityEditor.CustomEditor(typeof(CustomGrid))]
    public class CustomGridEditor : Editor
    {
        private void OnSceneGUI()
        {
            CustomGrid grid = (CustomGrid)target;
            if (grid == null) return;

            // 씬 뷰에 선을 그릴 색상 지정
            Handles.color = grid.gridColor;
            
            Vector3 center = grid.transform.position;
            Vector3 halfSize = grid.gridSize * 0.5f;
            float spacing = Mathf.Max(0.1f, grid.spacing); // 0 이하로 내려가 무한루프 도는 것 방지

            // 그리드의 최소/최대 경계값 계산
            float minX = center.x - halfSize.x;
            float maxX = center.x + halfSize.x;
            float minY = center.y - halfSize.y;
            float maxY = center.y + halfSize.y;
            float minZ = center.z - halfSize.z;
            float maxZ = center.z + halfSize.z;

            // 1. 측면(Side) 그리드 그리기
            if (grid.showSides)
            {
                // 앞면 & 뒷면에 세로줄 그리기
                for (float x = minX; x <= maxX; x += spacing)
                {
                    Handles.DrawLine(new Vector3(x, minY, minZ), new Vector3(x, maxY, minZ)); // 뒤
                    Handles.DrawLine(new Vector3(x, minY, maxZ), new Vector3(x, maxY, maxZ)); // 앞
                }

                // 좌측면 & 우측면에 세로줄 그리기
                for (float z = minZ; z <= maxZ; z += spacing)
                {
                    Handles.DrawLine(new Vector3(minX, minY, z), new Vector3(minX, maxY, z)); // 좌
                    Handles.DrawLine(new Vector3(maxX, minY, z), new Vector3(maxX, maxY, z)); // 우
                }

                // 모든 측면 벽에 가로줄(링) 그리기
                for (float y = minY; y <= maxY; y += spacing)
                {
                    // 상하 가시성 옵션에 따라 최상단/최하단 테두리 선 스킵 여부 결정
                    if (!grid.showTop && Mathf.Approximately(y, maxY)) continue;
                    if (!grid.showBottom && Mathf.Approximately(y, minY)) continue;

                    // 앞/뒤 가로선
                    Handles.DrawLine(new Vector3(minX, y, minZ), new Vector3(maxX, y, minZ));
                    Handles.DrawLine(new Vector3(minX, y, maxZ), new Vector3(maxX, y, maxZ));

                    // 좌/우 가로선
                    Handles.DrawLine(new Vector3(minX, y, minZ), new Vector3(minX, y, maxZ));
                    Handles.DrawLine(new Vector3(maxX, y, minZ), new Vector3(maxX, y, maxZ));
                }
            }

            // 2. 상단(Top) 천장 그리드 십자선 그리기
            if (grid.showTop)
            {
                DrawHorizontalPlane(minX, maxX, minZ, maxZ, maxY, spacing);
            }

            // 3. 하단(Bottom) 바닥 그리드 십자선 그리기
            if (grid.showBottom)
            {
                DrawHorizontalPlane(minX, maxX, minZ, maxZ, minY, spacing);
            }
        }

        // 수평 평면에 그리드 격자를 그려주는 편의용 함수
        private void DrawHorizontalPlane(float minX, float maxX, float minZ, float maxZ, float y, float spacing)
        {
            for (float x = minX; x <= maxX; x += spacing)
            {
                Handles.DrawLine(new Vector3(x, y, minZ), new Vector3(x, y, maxZ));
            }
            for (float z = minZ; z <= maxZ; z += spacing)
            {
                Handles.DrawLine(new Vector3(minX, y, z), new Vector3(maxX, y, z));
            }
        }
    }
#endif
}