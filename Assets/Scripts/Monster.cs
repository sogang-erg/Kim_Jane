using UnityEngine;

public class Monster : MonoBehaviour
{
    public Player target; // 플레이어를 추적하기 위한 변수
    public float speed = 0.5f; // 몬스터 이동 속도

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Player>(); // 태그로 찾기

        if (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized; // 방향 계산
            direction.y = 0; // y축 이동 제거 (수평 이동만)
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            } // 몬스터가 플레이어를 바라보도록 회전
            transform.position += direction * speed * Time.deltaTime; // 이동
        }
        else
        {
            Debug.Log("Player not found!");
        }
            return;
    }
}
