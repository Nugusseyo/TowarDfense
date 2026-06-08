// 0
// using UnityEngine;
//
// public class TankStateMachine : MonoBehaviour
// {
//     public float CurrentHealth;
//     public float StateTimer;
//     
//     void Start()
//     {
//         
//     }
//     
//     void Update()
//     {
//         
//     }
//     
//     public float GetNearestEnemyDistance()
//     {
//         return 0f;
//     }
// }

// 1 
using UnityEngine;

public class TankStateMachine : MonoBehaviour
{
    [HideInInspector] public float CurrentHealth = 100f;
    [HideInInspector] public float StateTimer; // StateTimer는 이 탱크 상태 머신이 현재 상태로 전환된 이후부터 얼마나 시간이 경과 했는지를 나타내는 용도로 사용
    
    public TankStateSO initialState;     // 탱크 상태 머신의 초기 상태를 지정하기 위한 변수
    private TankStateSO currentState;    // 현재 탱크의 상태를 저장할 currentState 변수
    
    // 2
    public EnemyRegistrySO enemyRegistry;

    private void Start()
    {
        ChangeState(initialState);
    }

    private void Update()
    {
        StateTimer += Time.deltaTime;
        currentState.OnUpdate(this);
        
        /*
         * 각각의 Transition 구조체에는 상태 전이에 필요한 조건을 담고 있는 ConditionSO
           타입의 SO 인스턴스가 들어 있습니다. 따라서 이 ConditionSO에
           정의된 CheckCondition 함수를 실행하면서 TankStateMachine 자신(this)을 인자
           로 넘기면, 현재 상태에서 다음 상태로 전이할 조건이 충족되었는지를 판단
         */
        
        foreach (var transition in currentState.transitions)
        {
            if (transition.condition.CheckCondition(this))
            {
                ChangeState(transition.toState);
                break;
            }
        }
    }

    public void ChangeState(TankStateSO newState)
    {
        currentState?.OnExit(this);
        currentState = newState;
        currentState.OnEnter(this);
        StateTimer = 0f;
    }

    public float GetNearestEnemyDistance()
    {
        // Debug.Log("GetNearestEnemyDistance: 적 탱크와의 거리를 계산합니다.");
        // return 0f;
        
        // 2
        var distance = float.MaxValue;
        if(enemyRegistry)
        {
            distance = enemyRegistry.GetClosestEnemyDistance(gameObject);
        }
        return distance;
    }
}