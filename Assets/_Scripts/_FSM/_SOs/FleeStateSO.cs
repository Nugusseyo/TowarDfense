using UnityEngine;

[CreateAssetMenu(fileName = "FleeStateSO", menuName = "Scriptable Objects/FSM/FleeStateSO")]
public class FleeStateSO : TankStateSO
{
    public float fleeRadius = 15f;          // 도망 반경(주변 반경 n미터내의 무작위 지점을 서치)
    public float moveSpeed = 8f;            // 기본 도망 속도
    public float reachThreshold = 0.5f;     // 목표지점에 도달한것으로 간주
    
    private Vector3 _fleeTarget;            // 도망 목표지점을 저장할 변수

    public override void OnEnter(TankStateMachine owner)
    {
        SetNewFleeTarget(owner);
        Debug.Log($"{owner.name} 도망 상태에 진입했습니다.");
    }

    public override void OnUpdate(TankStateMachine owner)
    {
        /*
         * OnEnter 함수에서 구한 도망 목표 위치(fleeTarget)를 향해 탱크를
         * 이동 및 회전시키는 로직을 구현
         */
        
        // 목표지점까지 이동과 회전
        var direction = (_fleeTarget - owner.transform.position).normalized;
        owner.transform.position += direction * (moveSpeed * Time.deltaTime);
        if (direction != Vector3.zero)
        {
            owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
        }
            
        // 목표지점까지 도달했을 때 다시 한번 도망 위치 설정
        var distance = Vector3.Distance(owner.transform.position, _fleeTarget);
        if (distance <= reachThreshold)
        {
            SetNewFleeTarget(owner);
        }
    }
    
    public override void OnExit(TankStateMachine owner)
    {
        Debug.Log($"{owner.name} 도망 상태에서 벗어났습니다.");
    }

    private void SetNewFleeTarget(TankStateMachine owner)
    {
        /*
         * Random.insideUnitCircle : (0, 0)을 중심으로 반지름이 1인 원 안의 임의의 위치를 Vector2 타입으로 반환
           반환되는 값은 x와 y 좌표 모두 -1에서 1 사이의 값을 가지며, 결과적으로 크기가 1 이하인 벡터
           즉, 방향과 상대적인 거리만 제공할 뿐, 실제 반경은 1에 고정 
         */
        
 
        var randomPoint = Random.insideUnitCircle * fleeRadius;
        _fleeTarget = owner.transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
    }
}