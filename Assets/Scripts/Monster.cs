using UnityEngine;
using UnityEngine.AI; // NavMeshAgent 사용을 위해 필요
using System.Collections; // 코루틴 사용을 위해 필요
using UnityEngine.UI; // Slider 사용을 위해 필요

[RequireComponent(typeof(NavMeshAgent))]

public class Monster : MonoBehaviour
{
    private Rigidbody _rb;
    Animator _animator;
    [SerializeField] private float _detectionRadius = 20f; // 탐지 반경 설정
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

    [SerializeField] private int maxHealth = 100; // 몬스터 최대 체력
    [SerializeField] private Slider healthBar; // 체력바 UI
    private int currentHealth; // 현재 체력
    private bool _isDead;
    [SerializeField] private ParticleSystem deathEffect;

    void Start()
    {
        _rb = GetComponent<Rigidbody>(); 
        _animator = GetComponent<Animator>();

        currentHealth = maxHealth; // 초기 체력을 최대 체력으로 설정
        healthBar.maxValue = maxHealth; // 체력바 UI 초기화
        healthBar.value = currentHealth; // 체력바 UI 초기화

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
        while (!_isDead)
        {
            CheckPlayerInArea();
            yield return new WaitForSeconds(1f); // 1초마다 체크
        }
    }

    IEnumerator CoLogic() // 코루틴 버전으로 실행
    {
        while (!_isDead)
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

        // if (_isPlayerInDetectionArea)
        // {
        //     Debug.Log("플레이어가 탐지 반경 안에 있습니다.");
        // }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);

    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        currentHealth -= damage; // 체력 감소
        currentHealth = Mathf.Max(currentHealth, 0);
        healthBar.value = currentHealth; // 체력바 UI 업데이트
        Debug.Log($"Monster가 {damage} 데미지를 받음. 현재 체력: {currentHealth}/{maxHealth}");
        if (currentHealth <= 0)
        {
            Debug.Log("Monster 사망");
            Die(); // 체력이 0 이하가 되면 사망 처리
        }
    }
    private void Die()
    {
        _isDead = true;
        _animator.SetBool("moving", false);
        _animator.SetTrigger("dying");

        if (_navMeshAgent != null && _navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.ResetPath();
        }

        StartCoroutine(CoFinishDeath());
    }

    private IEnumerator CoFinishDeath()
    {
        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("DYING"))
        {
            yield return null;
        }

        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        gameObject.SetActive(false); // 몬스터 비활성화
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.EndGame();
        }
    }

    public void ResetMonster()
    {
        _isDead = false;
        currentHealth = maxHealth;
        healthBar.value = currentHealth;
        _animator.ResetTrigger("dying");
        _animator.SetBool("moving", false);

        _state = State.Idle;
        _isPlayerInDetectionArea = false;

        if (_player == null)
        {
            GameObject playerObj =
                GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _player = playerObj.transform;
            }
        }

        if (_navMeshAgent != null &&
            _navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.ResetPath();
            _navMeshAgent.isStopped = false;
        }

        gameObject.SetActive(true);
        StartCoroutine(CoPerception());
        StartCoroutine(CoLogic());
    }

    public void PlayDeathEffect()
    {
        if (deathEffect != null)
        {
            deathEffect.Play();
        }
    }
}
