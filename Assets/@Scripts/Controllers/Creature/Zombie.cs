using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : Creature
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Slider healthBar;
    [SerializeField] private float detectionRadius = 20f;
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private float attackRange = 0.4f;
    [SerializeField] private float stopDistance = 1f; // 플레이어와 유지할 거리
    [SerializeField] private float attackCooldown = 0f;
    [SerializeField] private int attackDamage = 30;
    [SerializeField] private float hitDelay = 0.8f; // 공격 모션 시작 후 타격 시점

    private Animator _animator;
    private NavMeshAgent _agent;
    private Transform _zombie;
    private Player _player;
    private int _hp;
    private bool _isDead;
    private float _nextAttackTime;

    public override void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        _agent.speed = 1f;
        _agent.stoppingDistance = stopDistance;
        _agent.radius = 0.1f;

        base.Awake();
    }

    public override void init()
    {
        _nextAttackTime = 0f;
        _animator.ResetTrigger("attack");

        _isDead = false;
        _hp = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = _hp;
        }

        _animator.ResetTrigger("dying");
        _animator.SetBool("moving", false);

        if (_agent.isOnNavMesh)
        {
            _agent.isStopped = false;
            _agent.ResetPath();
        }
    }

    void OnEnable()
    {
        StartCoroutine(CoChase());
    }

    IEnumerator CoChase()
    {
        while (!_isDead)
        {
            if (_zombie == null)
            {
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null)
                {
                    _zombie = p.transform;
                    _player = p.GetComponentInParent<Player>();
                }
            }

            if (_zombie != null && _agent.isOnNavMesh)
            {
                float dist = Vector3.Distance(transform.position, _zombie.position);

                // 멈추는 지점보다 약간 더 먼 거리에서도 공겵이 나가야 함
                if (dist <= Mathf.Max(attackRange, stopDistance + 0.2f))
                {
                    FaceTarget();

                    AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);

                    bool isAttacking =
                        (state.IsName("ATTACK") && state.normalizedTime < 1f)
                        || (_animator.IsInTransition(0)
                        && _animator.GetNextAnimatorStateInfo(0).IsName("ATTACK"));

                    if (isAttacking)
                    {
                        _agent.isStopped = true;
                        _animator.SetBool("moving", false);
                        _nextAttackTime = Time.time + attackCooldown;
                    }
                    else
                    {
                        // 공격 범위 안에서도 stoppingDistance까지 계속 접근
                        _agent.isStopped = false;
                        _agent.SetDestination(_zombie.position);
                        _animator.SetBool(
                            "moving",
                            _agent.pathPending ||
                            _agent.remainingDistance > _agent.stoppingDistance
                        );

                        if (Time.time >= _nextAttackTime)
                        {
                            _animator.SetTrigger("attack");
                            StartCoroutine(CoDealDamage());
                            _nextAttackTime = Time.time + attackCooldown;
                        }
                    }
                }
                else if (dist <= detectionRadius)
                {
                    _agent.isStopped = false;
                    _agent.SetDestination(_zombie.position);
                    _animator.SetBool(
                        "moving",
                        _agent.pathPending ||
                        _agent.remainingDistance > _agent.stoppingDistance
                    );
                }
                else
                {
                    _agent.ResetPath();
                    _animator.SetBool("moving", false);
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    // 타격 시점에 플레이어가 아직 범위 안이면 피해를 줌
    IEnumerator CoDealDamage()
    {
        yield return new WaitForSeconds(hitDelay);

        if (_isDead || _player == null) yield break;

        float reach = Mathf.Max(attackRange, stopDistance + 0.2f) + 0.3f;
        if (Vector3.Distance(transform.position, _zombie.position) <= reach)
            _player.TakeDamage(attackDamage);
    }

    private void FaceTarget()
    {
        Vector3 dir = _zombie.position - transform.position;
        dir.y = 0f;

        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    public override void TakeDamage(int damage)
    {
        if (_isDead) return;

        _hp = Mathf.Max(_hp - damage, 0);

        if (healthBar != null)
            healthBar.value = _hp;

        if (_hp == 0)
            Die();
    }

    private void Die()
    {
        _isDead = true;
        _animator.SetBool("moving", false);

        _animator.ResetTrigger("attack");
        _animator.SetTrigger("dying");

        if (_agent.isOnNavMesh)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
        }

        StartCoroutine(CoFinishDeath());
    }

    IEnumerator CoFinishDeath()
    {
        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("DYING"))
            yield return null;

        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        FindFirstObjectByType<GameManager>()?.OnZombieDied(this);
        ObjectManager.Instance.Despawn(this);
    }

    public void PlayDeathEffect()
    {
        if (deathEffect != null)
        {
            deathEffect.Play();
        }
    }
}
