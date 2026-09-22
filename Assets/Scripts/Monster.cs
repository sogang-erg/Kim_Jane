using UnityEngine;
using UnityEngine.AI; // NavMeshAgent 사용을 위해 필요
using System.Collections; // 코루틴 사용을 위해 필요

[RequireComponent(typeof(NavMeshAgent))]

public class Monster : MonoBehaviour
{
    [SerializeField] private float _detectionRadius = 5f; // 탐지 반경 설정
    [SerializeField] private float _attackRadius = 2f; // 공격 반경 설정
    private Transform _player;
    private bool _isPlayerInDetectionArea = false;
    private bool _isPlayerInAttackArea = false;
    // private float sumTime = 0f;

    enum State // 상태 정의
    {
        Idle,
        Chase,
        Attack
    }

    [SerializeField] private State _state = State.Idle;

    private NavMeshAgent _navMeshAgent;

    void Start()
    {
        // Player 태그로 플레이어 찾기
        Debug.Log("Monster Start!");
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _player = playerObj.transform;
            Debug.Log("Player 찾음: " + _player.name);
        }
        else
        {
            Debug.Log("Player를 찾지 못 찾음!");
        }

        _navMeshAgent = GetComponent<NavMeshAgent>(); // NavMeshAgent 컴포넌트 가져오기
        Debug.Log("NavMeshAgent 활성화: " + _navMeshAgent.enabled);
        Debug.Log("NavMeshAgent가 NavMesh 위에 있음: " + _navMeshAgent.isOnNavMesh);

        _navMeshAgent.speed = 2f; // 이동 속도 설정
        _navMeshAgent.baseOffset = 1f; // NavMeshAgent의 기본 오프셋 설정
        _navMeshAgent.stoppingDistance = _attackRadius + 0.5f; // 공격 반경 + 여유 거리
    
        StartCoroutine(CoPerception()); // 코루틴 시작
        StartCoroutine(CoLogic()); // 코루틴 시작
    }

    void Update()
    {
        // // 이것도 복잡해지면 부담되는 행동
        // sumTime += Time.deltaTime; // 타이머 누적
        // if (sumTime >= 1f) // 1초마다 플레이어 탐지 체크
        // {
        //     sumTime = 0f; // 1초마다 체크 후 초기화
        //     CheckPlayerInArea();
        // }
    }

    IEnumerator CoPerception() // 코루틴 버전으로 실행
    {
        while (true)
        {
            CheckPlayerInArea();
            yield return new WaitForSeconds(1f); // 1초마다 체크
        }
    }

      IEnumerator CoLogic() // 코루틴 버전으로 실행
    {
        while (true)
        {
            Debug.Log("현재 State: " + _state); 
            switch (_state)
            {
                case State.Idle: // Idle 상태에서의 행동
                    if (_isPlayerInDetectionArea)
                    {
                        Debug.Log("Idle → Chase");
                        _state = State.Chase;
                    }
                    break;
                case State.Chase: // Chase 상태에서의 행동
                    Debug.Log("Chase 실행");
                    if (_isPlayerInDetectionArea)
                    {   // 플레이어가 탐지 반경 안에 있을 때 Chase 행동
                        // Vector3 dir = _player.position - transform.position; 
                        // transform.position += dir.normalized * 2f * Time.deltaTime;

                        _navMeshAgent.SetDestination(_player.position);
                        // yield 추가해도 됨

                        if (_isPlayerInAttackArea)
                        {
                            Debug.Log("Chase → Attack");
                            _state = State.Attack;
                        }
                    }
                    else
                    {
                        Debug.Log("Chase → Idle");
                        _state = State.Idle; 
                    }
                    break;
                case State.Attack: // Attack 상태에서의 행동
                    Debug.Log("공격 시작!!");
                    // 스킬 애니메이션 추가
                    yield return new WaitForSeconds(0.2f); // 공격 후 1초 대기
                    Debug.Log("데미지 판정");
                    // 피 깍는 코드 추가
                    yield return new WaitForSeconds(0.3f); // 공격 후 대기
                    _state = State.Chase; // 공격 후 다시 Chase 상태로 전환
                    break;
            }
            yield return null;
        }
    }

    private void CheckPlayerInArea()
    {
        if (_player == null) return;

        float distance = Vector3.Distance(transform.position, _player.position);
        _isPlayerInDetectionArea = distance <= _detectionRadius;
        _isPlayerInAttackArea = distance <= _attackRadius;

        if (_isPlayerInDetectionArea)
        {
            Debug.Log("플레이어가 탐지 반경 안에 있습니다.");
        }
    }

    // 디버그용: 탐지 범위 시각화
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }
}
