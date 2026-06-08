using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterStats))]
[CanEditMultipleObjects] // 여러 오브젝트 동시 편집 지원
public class CharacterStatsEditor : UnityEditor.Editor
{
    // 1. 프로퍼티를 담을 변수 선언
    SerializedProperty nameProp;
    SerializedProperty healthProp;
    SerializedProperty colorProp;

    void OnEnable()
    {
        // 2. 연결된 객체로부터 프로퍼티를 찾아 연결 (캐싱)
        nameProp = serializedObject.FindProperty("characterName");
        healthProp = serializedObject.FindProperty("health");
        colorProp = serializedObject.FindProperty("teamColor");
    }

    public override void OnInspectorGUI()
    {
        // 3. 실시간 데이터 동기화 시작
        serializedObject.Update();
    
        EditorGUILayout.LabelField("Character Settings", EditorStyles.boldLabel);
    
        // 4. 필드 제작 (직접 변수를 건드리는 게 아니라 '프로퍼티'를 통해 수정)
        EditorGUILayout.PropertyField(nameProp, new GUIContent("이름"));
    
        // 체력에 따라 슬라이더 색상이 변하는 인터페이스 (학생들 흥미 유발용)
        Rect r = EditorGUILayout.GetControlRect(checked(true), 20);
        EditorGUI.ProgressBar(r, healthProp.intValue / 100f, $"HP: {healthProp.intValue}");
        EditorGUILayout.PropertyField(healthProp, new GUIContent("체력"));
    
        EditorGUILayout.PropertyField(colorProp, new GUIContent("팀 색상"));
    
        // 5. 변경사항 적용 (가장 중요!)
        // 이 코드가 호출되는 순간 Undo 등록 및 Prefab 저장이 일어납니다.
        serializedObject.ApplyModifiedProperties();
    }
}
