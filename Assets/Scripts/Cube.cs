using UnityEngine;

public class Cube : MonoBehaviour
{
    public float triggerDistance = 2.0f; // 트리거 거리
    public float rotationSpeed = 90.0f; // 회전 속도 (도/초)

    GameObject player;
    GameObject door;

    bool isRotating = false; // 회전 중인지 여부

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // 태그 player 찾기
        door = GameObject.FindGameObjectWithTag("Door"); // 태그 door 찾기
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && !isRotating) // 플레이어가 존재하고 회전 중이 아닐 때만 거리 계산
        {
            float distance = Vector3.Distance(transform.position, player.transform.position); // 거리 계산
            if (distance <= triggerDistance) // 트리거 거리 이내이면 회전 시작
            {
                isRotating = true; // 회전 시작

                if (door != null) // 문이 존재하면 비활성화
                {
                    door.SetActive(false); // 문 비활성화
                }
            }
        }
        if (isRotating) // 회전 중이면 회전 처리
            {
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime); // Y축 회전
            }
    }
}
