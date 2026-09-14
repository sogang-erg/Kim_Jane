using UnityEngine;

public class Player : MonoBehaviour
{

    void Start()
    {
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space)) // 처음으로 눌러짐
        // {
        //     Debug.Log("GetKeyDown"); 
        // }

        // if (Input.GetKey(KeyCode.Space)) // 계속 누름
        // {
        //     Debug.Log("GetKey"); 
        // }

        // if (Input.GetKeyUp(KeyCode.Space)) // 누르지 않게 됨
        // {
        //     Debug.Log("GetKeyUp"); 
        // }

        // // Input Manager 축 활용
        // float v = Input.GetAxis("Vertical");
        // float h = Input.GetAxis("Horizontal");
        // Debug.Log($"Vertical {v}");
        // Debug.Log($"Horizontal {h}");

        // jump, Fire1 
        if (Input.GetButtonDown("Jump"))
        { 
            Debug.Log("Jump");
        }
        if (Input.GetButtonDown("Fire1"))
        { 
            Debug.Log("Fire1");
        }
    }
}
