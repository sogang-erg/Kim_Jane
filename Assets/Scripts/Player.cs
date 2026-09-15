using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private InputAction _testAction; 

    void Start()
    {
        _testAction = InputSystem.actions.FindAction("Player/Test"); // 액션 가져오기

        if (_testAction == null)
        {
            Debug.LogError("Test Action을 찾지 못했습니다.");
            return;
        }

        _testAction.Enable(); // 활성화
        Debug.Log("Test Action을 찾았습니다.");

        _testAction.performed += OnPerformed; // 이벤트 방식으로 전달해 받음
        _testAction.canceled += OnCancelled; // 이벤트 방식으로 전달해 받음
    }

    void OnPerformed(InputAction.CallbackContext context)
    { 
        Debug.Log("Performed"); 
    }

    void OnCancelled(InputAction.CallbackContext context)
    { 
        Debug.Log("Cancelled"); 
    }

    void Update() // polling 방식
    {
        // if (_testAction.WasPerformedThisFrame()) // GetKeyDown에 해당
        // {
        //     Debug.Log("Performed");
        // }

        // if (_testAction.IsPressed()) // GetKey에 해당
        // {
        //     Debug.Log("Pressed");
        // }

        // if (_testAction.WasReleasedThisFrame()) // GetKeyUp에 해당
        // {
        //     Debug.Log("Released");
        // }
    }
}
