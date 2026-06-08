using UnityEngine;

[CreateAssetMenu(fileName = "SearchStateSO", menuName = "Scriptable Objects/FSM/SearchStateSO")]
public class SearchStateSO : TankStateSO
{
    public float rotationSpeed = 90f;

    public override void OnEnter(TankStateMachine owner)
    {
        Debug.Log($"{owner.name} 탐색 상태에 진입했습니다.");
    }

    public override void OnUpdate(TankStateMachine owner)
    {
        owner.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public override void OnExit(TankStateMachine owner)
    {
        Debug.Log($"{owner.name} 탐색 상태에서 벗어났습니다.");
    }
}