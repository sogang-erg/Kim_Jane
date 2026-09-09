using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Finish : MonoBehaviour
{
    public float finishDistance = 2.0f;

    GameObject player;

    void Start()
    {
        // player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        if (distance <= finishDistance)
        {
            Debug.Log("Game Clear!");

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}