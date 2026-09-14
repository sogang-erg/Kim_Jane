using UnityEngine;

public class Player : Character
{
    public float speed = 5; // 언제든 수정 가능하도록 설정
    Transform monster;

    void Start()
    {
        // transform.position += new Vector3(1, 1, 1); // Vector3 벡터 통째로 변환, 덧셈 가능
        monster = GameObject.Find("Monster").transform; // 몬스터 찾기
    }

    void Update()
    {
        // Vector3 position = transform.position; // 통채로 복붙
        // // position.z += 1; // 1씩 증가-하드 코딩,,
        // position.z += Time.deltaTime * speed; // 이게 나음
        // transform.position = position; // 다시 갖다 놓음

        // transform.position += new Vector3(0, 0, 1) * speed * Time.deltaTime; // z 이동
        // transform.position += Vector3.forward * speed * Time.deltaTime; // z 이동(위와 동일)

        // // Vector(AB) = B - A
        // Vector3 dir = monster.position - transform.position; 
        // float distance = dir.magnitude; // .magnitude: 벡터 크기 반환 기능
        // if (distance < 5)
        // { 
        //     Debug.Log("Attack!"); // 공격!!
        // }
        // else
        // { 
        //     transform.position += dir.normalized * speed * Time.deltaTime; 
        //     transform.LookAt(monster); // 바라보기 회전
        // }

        // dir.Normalize(); // 원본 그대로 가져와서 변환 방법
        // Vector3 dirNormalized = dir.normalized; // .normalized: 벡터 방향 반환 기능
        // transform.position += dir.normalized * speed * Time.deltaTime; // 이동

        // 키보드로 이동
        Vector3 dir = Vector3.zero; // 누적 설정
        if (Input.GetKey(KeyCode.W))
            dir += Vector3.forward; 
        if (Input.GetKey(KeyCode.S))
            dir += Vector3.back; 
        if (Input.GetKey(KeyCode.A))
            dir += Vector3.left; 
        if (Input.GetKey(KeyCode.D))
            dir += Vector3.right; 
        transform.position += dir.normalized * speed * Time.deltaTime; // 정규화로 길이 날리기
        // transform.Translate(dir.normalized * speed * Time.deltaTime); // 이 방법으로도 구현하곤 함
    }
}
