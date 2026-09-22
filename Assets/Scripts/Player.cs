using UnityEngine;
using UnityEngine.AI;

// Collider(IsTrigger X): Collision 용도
// Collider(IsTrigger O): Trigger 용도
// Rigidbody(IsKinematic X): 물리적 영향을 받음 (힘, 중력, 충돌 등)
// Rigidbody(IsKinematic O): 물리적 영향을 받지 않음 (인형을 알아서 움직이는 용도)

// Case Study 1: Player vs Wall
// A: Collider(IsTrigger X) + Rigidbody(IsKinematic X)
// B: Collider(IsTrigger X)

// Case Study 2: Player vs Portal
// A: Collider(IsTrigger ?) + Rigidbody(IsKinematic ?)
// B: Collider(IsTrigger O)

// Case Study 3: Player vs Monster (MMO) << 벽 충돌 없음, 유닛 간 충돌 없음
// A: Collider(IsTrigger X) + Rigidbody(IsKinematic O)
// B: Collider(IsTrigger X) + Rigidbody(IsKinematic O)

// Case Study 4: Player vs Monster (Vampire Survival) 
// A: Collider(IsTrigger X) + Rigidbody(IsKinematic X)
// B: Collider(IsTrigger X) + Rigidbody(IsKinematic X)

public class Player : MonoBehaviour
{
    private float _searchRadius = 5f;
    private float _searchTimer = 0f;
    private float _searchInterval = 1f;

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>(); 
    }

    // Shape Casting
    void Update()
    {
        _searchTimer += Time.deltaTime;
        if (_searchTimer >= _searchInterval)
        {
            _searchTimer = 0f;
            SearchNearbyColliders();
            PickObject();
        }
    }

    // 구로 주변 물체 탐지
    void SearchNearbyColliders()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _searchRadius);

        foreach (Collider col in colliders)
        {
            if (col.transform != gameObject)
            Debug.Log($"Nearby Collider: {col.gameObject.name}");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _searchRadius);
    }

    // RayCast 발동
    void PickObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log($"Picked Object: {hit.collider.gameObject.name}");
        }
        else
        {
            Debug.Log("No object picked");
        }
    }

    void FixedUpdate()
    {
        Vector3 dir = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            dir += Vector3.forward;
        if (Input.GetKey(KeyCode.S))
            dir += Vector3.back;
        if (Input.GetKey(KeyCode.A))
            dir += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            dir += Vector3.right;

        if (_rb != null)
            _rb.MovePosition(transform.position + dir.normalized * Time.deltaTime * 5f);

    }

    // 충돌 이벤트
    public void OnCollisionEnter(Collision collision) // 시작
    {
        Debug.Log($"OnCollisionEnter({collision.gameObject.name})");
    }

    public void OnCollisionStay(Collision collision) // 진행 
    {
        Debug.Log($"OnCollisionStay({collision.gameObject.name})");
    }

    public void OnCollisionExit(Collision collision) // 끝
    {
        Debug.Log($"OnCollisionExit({collision.gameObject.name})");
    }

    // 트리거 이벤트
    public void OnTriggerEnter(Collider other) // 시작
    {
        Debug.Log($"OnTriggerEnter({other.gameObject.name})");
    }

    public void OnTriggerStay(Collider other) // 진행
    {
        Debug.Log($"OnTriggerStay({other.gameObject.name})");
    }

    public void OnTriggerExit(Collider other) // 끝
    {
        Debug.Log($"OnTriggerExit({other.gameObject.name})");
    }
}
