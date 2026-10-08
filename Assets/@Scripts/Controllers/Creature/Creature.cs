using UnityEngine;
using UnityEngine.AI;

public class Creature : ObjectBase
{
    public void Place(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
            agent.Warp(pos); // NavMeshAgent는 Warp로 옮겨야 위치가 어긋나지 않음
    }

    public virtual void TakeDamage(int damage)
    {
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
