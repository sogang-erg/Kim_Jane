using UnityEngine;
using UnityEngine.InputSystem; // InputSystem 사용

public class Player : MonoBehaviour
{
    public int hp = 100;
    public float speed = 5.0f; // 이동 속도

    // [SerializeField]
    // string playerName;
    // [SerializeField]
    // int health;
    // [SerializeField]
    // int attack;

    // public string playerName = "Player";
    // public int health = 100;
    // public int attack = 10;
    // float sumTime = 0;
    // public Player target; 

    void Start()
    {
        // gameObject.GetComponent<Transform>().position = new Vector3(10, 10, 10);
        // GetComponent<Transform>().position = new Vector3(10, 10, 10);
        // transform.position = new Vector3(10, 10, 10); // 동일 코드
    }

    void Update()
    {
        float x = 0f; // 이동 방향 초기화
        float z = 0f; // 이동 방향 초기화

        if (Keyboard.current.wKey.isPressed)
        {
            x = 1f;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            x = -1f;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            z = 1f;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            z = -1f;
        }
        Vector3 direction = new Vector3(x, 0f, z); // 이동 방향 벡터 생성
        transform.position += direction * speed * Time.deltaTime; // 이동 처리

        // sumTime += Time.deltaTime;
        // if (sumTime >= 1)
        // {
        //     sumTime = 0;
        //     // GenerateHP();
        //     Attack();
        // }
    }

    // void GenerateHP()
    // {
    //     this.health++;
    // }

    //     void Attack()
    // {
    //     if (target != null)
    //     {
    //         target.health -= attack;
    //     }
    // }
}
