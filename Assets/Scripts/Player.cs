using UnityEngine;

public class Player : Character
{
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
