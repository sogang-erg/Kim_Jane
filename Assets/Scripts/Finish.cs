using UnityEngine;

#if UNITY_EDITOR // UnityEditor 네임스페이스 사용
using UnityEditor;
#endif

public class Finish : MonoBehaviour
{
    public float finishDistance = 2.0f; // 게임 클리어 거리

    GameObject player; // player 오브젝트 참조

    void Start()
    {
        // player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player"); // 태그 player 찾기
            return;
        }

        float distance = Vector3.Distance(transform.position, player.transform.position); // 거리 계산

        if (distance <= finishDistance) // 트리거 거리 이내이면 게임 클리어
        {
            Debug.Log("Game Clear!");

#if UNITY_EDITOR
            EditorApplication.isPlaying = false; // 에디터에서 실행 중이면 정지
#else
            Application.Quit(); // 빌드된 게임이면 종료
#endif
        }
    }
}