// using UnityEngine;

// public class FollowCamera : MonoBehaviour
// {
//     public Transform player;
//     public Vector3 offset;

//     void Start()
//     {
//     }

//     // Update is called once per frame
//     void LateUpdate()
//     {
//         transform.position = player.position + offset;
//         Vector3 target = player.position + Vector3.up * 1.5f;
//         transform.LookAt(target);
//     }
// }

using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    [SerializeField] private float sensitivity = 3f;

    private float _yaw;

    void LateUpdate()
    {
        if (Mouse.current != null && Mouse.current.rightButton.isPressed)
            _yaw += Mouse.current.delta.ReadValue().x * sensitivity;

        Quaternion rot = Quaternion.Euler(0f, _yaw, 0f);
        transform.position = player.position + rot * offset;

        Vector3 target = player.position + Vector3.up * 1.5f;
        transform.LookAt(target);
    }
}
