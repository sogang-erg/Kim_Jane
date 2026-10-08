using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem; // InputSystem 사용
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody _rb;
    Animator _animator;

    private InputAction _testAction; 

    [SerializeField] private int attackDamage = 10; // 공격력
    [SerializeField] private float attackRange = 1.5f; // 공격 범위

    private bool _hitMonster;
    [SerializeField] private ParticleSystem hitEffect;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Slider healthBar;
    private int _hp;
    private bool _isDead;

    private Transform _cam;

    void Start()
    {
        _rb = GetComponent<Rigidbody>(); 

        _animator = GetComponent<Animator>();
        _testAction = InputSystem.actions.FindAction("Player/Test"); // 액션 가져오기
        _testAction.Enable(); // 활성화

        _cam = Camera.main.transform;

        ResetHealth();
    }

    public void ResetHealth()
    {
        _isDead = false;
        _hp = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = _hp;
        }
    }

    public void Heal(int amount)
    {
        if (_isDead) return;

        _hp = Mathf.Min(_hp + amount, maxHealth);

        if (healthBar != null)
            healthBar.value = _hp;
    }

    public void TakeDamage(int damage)
    {
        if (_isDead) return;

        _hp = Mathf.Max(_hp - damage, 0);

        if (healthBar != null)
            healthBar.value = _hp;

        if (_hp == 0)
        {
            _isDead = true;
            FindFirstObjectByType<GameManager>()?.OnPlayerDied();
        }
    }

    void Update()
    {
        Vector2 input = _testAction.ReadValue<Vector2>();
        // Vector3 dir = new Vector3(input.x, 0, input.y);
        Vector3 forward = Vector3.ProjectOnPlane(_cam.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(_cam.right, Vector3.up).normalized;
        Vector3 dir = right * input.x + forward * input.y;

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
        transform.position += dir * 10f * Time.deltaTime;

        // 이동 여부에 따라 IDLE / MOVE 애니메이션 전환
        AnimatorStateInfo currentInfo = _animator.GetCurrentAnimatorStateInfo(0);
        bool isAttacking = currentInfo.IsName("ATTACK") && currentInfo.normalizedTime < 1f;
        // 공격 중이 아닐 때만 이동에 따른 애니메이션 전환
        if (!isAttacking)
        {
            bool isMoving = dir != Vector3.zero;
            _animator.SetBool("moving", isMoving);
        }

        // 마우스 왼쪽 버튼으로 공격
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame
            && !(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()))
        {
            if (!isAttacking)
            {
                _animator.SetTrigger("attack");
                Attack(); // 공격 실행
            }
        }
    }

    void Attack()
    {
        _hitMonster = false;

        Vector3 attackCenter = transform.position + transform.forward * attackRange;
        Collider[] hits = Physics.OverlapSphere(attackCenter, attackRange);

        foreach (Collider hit in hits)
        {
            Creature creature = hit.GetComponentInParent<Creature>();

            if (creature != null)
            {
                creature.TakeDamage(attackDamage);
                _hitMonster = true;
                Debug.Log("Creature를 맞춤");
                break;
            }
        }
    }

    public void PlayHitEffect()
    {
        Debug.Log($"Animation Event 실행 / _hitMonster = {_hitMonster}");

        if (!_hitMonster)
            return;

        Debug.Log($"hitEffect = {hitEffect}");

        hitEffect.Play();

        Debug.Log("Hit Effect 재생");

        _hitMonster = false;
    }
}
