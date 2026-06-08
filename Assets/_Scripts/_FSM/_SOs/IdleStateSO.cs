using UnityEngine;

[CreateAssetMenu(fileName = "IdleStateSO", menuName = "Scriptable Objects/FSM/IdleStateSO")]
public class IdleStateSO : TankStateSO
{
    public override void OnEnter(TankStateMachine owner)
    {
        Debug.Log($"{owner.name} 대기 상태에 진입했습니다.");
    }

    public override void OnUpdate(TankStateMachine owner)
    {
    }

    public override void OnExit(TankStateMachine owner)
    {
        Debug.Log($"{owner.name} 대기 상태에서 벗어났습니다.");
    }
}