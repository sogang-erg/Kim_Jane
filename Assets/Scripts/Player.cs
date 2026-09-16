using UnityEngine;
using UnityEngine.InputSystem; // InputSystem 사용

public class Player : MonoBehaviour
{
    private InputAction _testAction; 

    void Start()
    {
        _testAction = InputSystem.actions.FindAction("Player/Test"); // 액션 가져오기
        _testAction.Enable(); // 활성화
    }

    void Update()
    {
        Vector2 input = _testAction.ReadValue<Vector2>();
        Vector3 dir = new Vector3(input.x, 0, input.y);
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
        transform.position += dir * 5.0f * Time.deltaTime;
    }
}
