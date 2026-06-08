using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemGenerator))]
public class ItemGeneratorEditor : UnityEditor.Editor
{
    SerializedProperty nameProp;
    SerializedProperty idProp;
    SerializedProperty typeProp;
    SerializedProperty valueProp;

    void OnEnable()
    {
        nameProp = serializedObject.FindProperty("newItemName");
        idProp = serializedObject.FindProperty("newItemID");
        typeProp = serializedObject.FindProperty("newItemType");
        valueProp = serializedObject.FindProperty("newValue");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("🎁 아이템 데이터 생성기", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 1. 데이터 입력 필드들
        EditorGUILayout.PropertyField(nameProp, new GUIContent("아이템 이름"));
        EditorGUILayout.PropertyField(idProp, new GUIContent("아이템 ID"));
        EditorGUILayout.PropertyField(typeProp, new GUIContent("아이템 종류"));
        EditorGUILayout.PropertyField(valueProp, new GUIContent("능력치/효과량"));

        EditorGUILayout.Space(15);

        // 2. 난이도 상: 유니티 인스펙터에 버튼 배치하기
        // GUILayout.Button은 클릭되는 순간 true를 반환합니다.
        if (GUILayout.Button("새 아이템 파일 생성 (.asset)", GUILayout.Height(35)))
        {
            // 버튼 클릭 시 실행할 로직 함수 호출
            CreateItemAsset();
        }

        serializedObject.ApplyModifiedProperties();
    }

    // 파일 생성 핵심 로직
    private void CreateItemAsset()
    {
        // 1. 메모리 상에 새로운 ItemData 객체(ScriptableObject) 생성
        ItemData newItem = ScriptableObject.CreateInstance<ItemData>();

        // 2. 현재SerializedProperty에 입력된 값들을 새 객체에 대입
        newItem.itemName = nameProp.stringValue;
        newItem.itemID = idProp.intValue;
        newItem.type = (ItemData.ItemType)typeProp.enumValueIndex;
        newItem.value = valueProp.intValue;

        // 3. 저장할 경로 지정 (Assets/New Item.asset)
        // 실제 프로젝트라면 중복 방지를 위해 ID나 이름을 경로에 섞어줍니다.
        string path = $"Assets/{newItem.itemName}_{newItem.itemID}.asset";

        // 4. 유니티 AssetDatabase 시스템을 이용해 파일로 물리적 저장
        AssetDatabase.CreateAsset(newItem, path);
        AssetDatabase.SaveAssets();

        // 5. 생성 완료 후 프로젝트 창에서 해당 파일을 자동으로 선택(포커스)해주는 편의 기능
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newItem;

        // 알림창 띄우기
        Debug.Log($"<color=green>[성공]</color> 아이템 파일이 생성되었습니다: {path}");
    }
}