using UnityEngine;
using UnityEngine.AI; // NavMeshAgent 사용을 위해 필요
using System.Collections; // 코루틴 사용을 위해 필요

[RequireComponent(typeof(NavMeshAgent))]

public class Monster : MonoBehaviour
{
    private Rigidbody _rb;
    Animator _animator;
    [SerializeField] private float _detectionRadius = 10f; // 탐지 반경 설정
    private Transform _player;
    private bool _isPlayerInDetectionArea = false;

    enum State // 상태 정의
    {
        Idle,
        Chase
    }

    [SerializeField] private State _state = State.Idle;

    private NavMeshAgent _navMeshAgent;

    // public Player target; // 플레이어를 추적하기 위한 변수
    // public float speed = 0.5f; // 몬스터 이동 속도

    void Start()
    {
        _rb = GetComponent<Rigidbody>(); 
        _animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _player = playerObj.transform;
        }

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = 2f; // 이동 속도 설정
        _navMeshAgent.stoppingDistance = 2f; // 여유 거리

        StartCoroutine(CoPerception()); // 코루틴 시작
        StartCoroutine(CoLogic()); // 코루틴 시작
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
            switch (_state)
            {
                case State.Idle: // Idle 상태에서의 행동
                    _animator.SetBool("moving", false); // Idle 상태에서는 이동 애니메이션 끄기
                    if (_isPlayerInDetectionArea)
                    {
                        _state = State.Chase;
                    }
                    break;
                case State.Chase: // Chase 상태에서의 행동
                    _animator.SetBool("moving", true); // Chase 상태에서는 이동 애니메이션 켜기
                    if (_isPlayerInDetectionArea)
                    {   // 플레이어가 탐지 반경 안에 있을 때 Chase 행동
                        if (_navMeshAgent.isOnNavMesh)
                        {
                            _navMeshAgent.SetDestination(_player.position);

                            // 
                            bool isMoving = _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance;
                            _animator.SetBool("moving", isMoving);
                        }
                    }
                    else
                    {
                        Debug.Log("Chase → Idle");
                        _animator.SetBool("moving", false); // Chase → Idle 전환 시 이동 애니메이션 끄기
                        _state = State.Idle; 
                    }
                    break; // Chase 상태 종료
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void CheckPlayerInArea()
    {
        if (_player == null) return;

        float distance = Vector3.Distance(transform.position, _player.position);
        _isPlayerInDetectionArea = distance <= _detectionRadius;

        if (_isPlayerInDetectionArea)
        {
            Debug.Log("플레이어가 탐지 반경 안에 있습니다.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);

    }
}
