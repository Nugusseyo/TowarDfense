using UnityEditor;

namespace Editor
{
    // TankStateMachine 컴포넌트의 인스펙터(Inspector) UI를 커스터마이징하는 에디터 스크립트입니다.
    // 이 스크립트가 있으면 기본 인스펙터 대신 원하는 방식으로 속성을 표시하거나,
    // ScriptableObject의 인스펙터까지 함께 보여줄 수 있습니다.
    [CustomEditor(typeof(TankStateMachine))]
    public class TankStateMachineEditor : UnityEditor.Editor
    {
        // TankStateMachine 안의 "initialState" 필드를 인스펙터에서 제어하기 위한 SerializedProperty입니다.
        private SerializedProperty _initialStateProp;

        // TankStateMachine 안의 "enemyRegistry" 필드를 인스펙터에서 제어하기 위한 SerializedProperty입니다.
        private SerializedProperty _enemyRegistryProp;

        // initialState로 연결된 ScriptableObject(TankStateSO)의 인스펙터를 직접 그리기 위해 사용하는 임시 에디터입니다.
        private UnityEditor.Editor _initialStateSoEditor;

        private void OnEnable()
        {
            // 인스펙터가 활성화될 때, 직렬화된 필드를 찾아 캐싱해 둡니다.
            // 문자열은 실제 TankStateMachine 클래스의 필드 이름과 정확히 일치해야 합니다.
            _initialStateProp = serializedObject.FindProperty("initialState");
            _enemyRegistryProp = serializedObject.FindProperty("enemyRegistry");
        }

        public override void OnInspectorGUI()
        {
            // 현재 오브젝트의 최신 값을 반영합니다.
            serializedObject.Update();

            // initialState 필드를 기본 필드 형태로 표시합니다.
            EditorGUILayout.PropertyField(_initialStateProp);

            // initialState가 TankStateSO를 참조하고 있는지 확인합니다.
            var soAsset = _initialStateProp.objectReferenceValue as TankStateSO;
            if (soAsset)
            {
                // 참조된 SO가 바뀌었으면 기존 에디터를 재사용하지 않고 새로 만듭니다.
                // 이렇게 해야 다른 SO를 선택했을 때 인스펙터가 올바르게 갱신됩니다.
                if (!_initialStateSoEditor || _initialStateSoEditor.target != soAsset)
                {
                    // 이전 에디터가 있으면 메모리 누수를 막기 위해 정리합니다.
                    if (_initialStateSoEditor)
                        DestroyImmediate(_initialStateSoEditor);

                    // 선택된 ScriptableObject용 에디터를 생성합니다.
                    _initialStateSoEditor = CreateEditor(soAsset);
                }

                // ScriptableObject의 인스펙터를 현재 인스펙터 안에 그대로 그립니다.
                _initialStateSoEditor.OnInspectorGUI();
            }

            // enemyRegistry 필드도 인스펙터에 표시합니다.
            EditorGUILayout.PropertyField(_enemyRegistryProp);

            // 사용자가 수정한 내용을 실제 오브젝트에 반영합니다.
            serializedObject.ApplyModifiedProperties();
        }

        private void OnDisable()
        {
            // 에디터가 비활성화될 때 생성했던 임시 에디터를 정리합니다.
            // Unity Editor에서는 이런 정리를 해줘야 불필요한 객체가 남지 않습니다.
            if (_initialStateSoEditor != null)
                DestroyImmediate(_initialStateSoEditor);
        }
    }
}