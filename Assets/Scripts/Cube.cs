using UnityEngine;

public class Cube : MonoBehaviour
{
    public float triggerDistance = 2.0f;
    public float rotationSpeed = 90.0f;

    GameObject player;
    GameObject door;

    bool isRotating = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        door = GameObject.FindGameObjectWithTag("Door");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && !isRotating)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= triggerDistance)
            {
                isRotating = true;

                if (door != null)
                {
                    door.SetActive(false);
                }
            }
        }
        if (isRotating)
            {
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
    }
}
