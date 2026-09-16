using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private InputAction _testAction; 

    void Start()
    {
        InputActionMap inputActionMap =InputSystem.actions.FindActionMap("Player"); // 액션맵 가져오기
        inputActionMap.Disable(); // 액션맵 비활성화

        _testAction = InputSystem.actions.FindAction("Player/Test"); // 액션 가져오기

        if (_testAction == null)
        {
            Debug.LogError("Test Action을 찾지 못했습니다.");
            return;
        }

        _testAction.Enable(); // 활성화

    }

    void Update()
    {
        Vector2 move = _testAction.ReadValue<Vector2>(); // Vector2 정보 리턴
        Debug.Log(move);

    }
}
