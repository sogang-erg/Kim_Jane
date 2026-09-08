using UnityEngine;

public class Monster : MonoBehaviour
{
    public Player target; // 플레이어를 추적하기 위한 변수

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
            return;
            
        // target = GameObject.Find("Player").GetComponent<Player>(); // 오브젝트 이름으로 찾기
        // target = GameObject.FindWithTag("Player").GetComponent<Player>(); // 태그로 찾기
        target = GameObject.FindFirstObjectByType<Player>(); // 타입으로 찾기
    }
}
