using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.position + offset;
        Vector3 target = player.position + Vector3.up * 1.5f;
        transform.LookAt(target);
    }
}
