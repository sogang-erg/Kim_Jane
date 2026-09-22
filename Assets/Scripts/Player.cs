using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;

    int IDLE = Animator.StringToHash("IDLE"); // 해쉬 값
    int MOVE = Animator.StringToHash("MOVE");
    int ATTACK = Animator.StringToHash("ATTACK");

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Shape Casting
    void Update()
    {
        if (Input.GetKey(KeyCode.F1))
            // animator.Play("IDLE"); // string 문자류는 기본적으로 느림
            // animator.Play(IDLE); // 해쉬 값 사용
            animator.SetBool("moving", false); // 파라미터 연동

        if (Input.GetKey(KeyCode.F2))
            // animator.Play("MOVE");
            // animator.Play(MOVE); // 해쉬 값 사용
            animator.SetBool("moving", true); // 파라미터 연동

        if (Input.GetKey(KeyCode.F3))
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0); // Layer 상태 정보
            // if (info.IsName ("ATTACK") == false || info.normalizedTime >= 1) // 애니메이션 재생 끝
            //     animator.Play("ATTACK", 0, 0); // 애니메이션 이름 기입, Layer번호, 재생옵션(-1:다시안틂, 0:리셋, 0.5f중간, 1:?)
            if (info.shortNameHash == ATTACK == false || info.normalizedTime >= 1) // 애니메이션 재생 끝
                // animator.Play(ATTACK, 0, 0); // 해쉬 값 사용
                animator.SetTrigger("attack"); // 파라미터 연동
        }
    }

    public void TriggerOnDamaged()
    {
        Debug.Log("Triggered On Damaged");
    }
    
}
