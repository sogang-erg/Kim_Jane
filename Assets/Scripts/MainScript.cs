using UnityEngine;

public class MainScript : MonoBehaviour
{
    public Mesh playerMesh; 
    public Material playerMaterial; 
    public GameObject player;
    public Material cubeMaterial; // 머티리얼 지정
    
    void Start()
    {
        player = CreatePlayer().gameObject; // gameObject 가져오기
        player.transform.position = new Vector3(-4, 1, -2);

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "TargetCube";
        cube.transform.position = new Vector3(5, 2, -2); // 위치 조정
        cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // 크기 조정
        cube.GetComponent<MeshRenderer>().material = cubeMaterial;
        cube.AddComponent<Cube>(); // Cube 스크립트 추가

        // player.GetComponent<Player>().hp = 0;
        // GameObject.Destroy(player, 3); // player Script 삭제
        // GameObject.Destroy(player.gameObject, 5); // player 오브제 5초 뒤 삭제
        // player.gameObject.SetActive(false); // 비활성화
        // player.enabled = false; // player Script 비활성화
    }

    Player CreatePlayer()
    {
        GameObject go = new GameObject();
        go.name = "Player";
        go.tag = "Player"; // 태그 지정

        Rigidbody rb = go.AddComponent<Rigidbody>(); // Rigidbody 컴포넌트 추가
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation; // 회전 제한
        // Rigidbody 컴포넌트는 물리 엔진과 상호작용을 위해 필요: collider와 함께 사용하여 충돌 감지 및 물리적 움직임을 처리할 수 있음
        go.AddComponent<CapsuleCollider>(); // CapsuleCollider 컴포넌트 추가

        MeshFilter meshFilter = go.AddComponent<MeshFilter>(); // MeshFilter 컴포넌트 추가
        meshFilter.mesh = playerMesh;
        MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>(); // MeshRenderer 컴포넌트 추가
        meshRenderer.material = playerMaterial;
        Player player = go.AddComponent<Player>(); // Player 스크립트 추가
        // player.hp = 200; // 다른 값으로 수정 가능
        // return go;
        return player;
    }

    void Update()
    {
        // Debug.Log(Time.deltaTime);
        // if (player != null) // unity fake null = null이 아니라면 
        // { 
        //     // player.hp--;
        //     player.GetComponent<Player>().hp--; // GameObject용 ~Player script >> fake null 없이는 에러
        // }
    }
}
