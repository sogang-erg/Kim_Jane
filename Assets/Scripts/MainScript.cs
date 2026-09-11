using UnityEngine;

public class MainScript : MonoBehaviour
{
    // public GameObject prefab; // 프리팹 드래그앤드롭 연결 버전
    Player player; // 인스턴스 변수 설정
    Character target; // 더 상위 상속 활용

    void Start()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Player"); // 로딩 버전
        GameObject go = GameObject.Instantiate(prefab);
        go.name = prefab.name; 

        player = go.GetComponent<Player>(); // player 참조값을 따라 player 관리
        target = go.GetComponent<Player>(); // target 참조값을 따라 target 관리

    }

    void Update()
    {
    }
}
