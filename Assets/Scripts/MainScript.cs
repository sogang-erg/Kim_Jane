using UnityEngine;

public class MainScript : MonoBehaviour
{
    public Material cubeMaterial; // 머티리얼 지정
    private GameObject go; // Player 오브젝트 참조
    public Player player;
    private GameObject monsterGo; // Monster 오브젝트 참조
    public Monster monster;

    void Start()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Player"); // Player 프래팹 로드
        go = GameObject.Instantiate(prefab); // Player 프래팹 인스턴스 생성
        go.name = prefab.name; // 이름 지정
        go.tag = "Player"; // 태그 지정

        GameObject monsterPrefab = Resources.Load<GameObject>("Prefabs/Zombie"); // Monster 프래팹 로드
        monsterGo = GameObject.Instantiate(monsterPrefab); // Monster 프래팹 인스턴스 생성
        monsterGo.name = monsterPrefab.name; // 이름 지정
        // monsterGo.tag = "Monster"; // 태그 지정

        player = CreatePlayer(); // CreatePlayer() 호출 참조
        player.transform.position = new Vector3(-4, 0, -2);
        player.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f); // 크기 조정

        monster = CreateMonster(); // CreateMonster() 호출 참조
        monster.transform.position = new Vector3(-6, 0, -2);
        monster.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f); // 크기 조정

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "TargetCube";
        cube.transform.position = new Vector3(5, 1.5f, -2); // 위치 조정
        cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // 크기 조정
        cube.GetComponent<MeshRenderer>().material = cubeMaterial;
        cube.AddComponent<Cube>(); // Cube 스크립트 추가
    }

    Player CreatePlayer()
    {
        Rigidbody rb = go.AddComponent<Rigidbody>(); // Rigidbody 컴포넌트 추가
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation; // 회전 제한
        // Rigidbody 컴포넌트는 물리 엔진과 상호작용을 위해 필요: collider와 함께 사용하여 충돌 감지 및 물리적 움직임을 처리할 수 있음
        player = go.AddComponent<Player>(); // Player 스크립트 추가
        return player;
    }

    Monster CreateMonster()
    {
        Rigidbody rb = monsterGo.AddComponent<Rigidbody>(); // Rigidbody 컴포넌트 추가
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation; // 회전 제한
        // Rigidbody 컴포넌트는 물리 엔진과 상호작용을 위해 필요: collider와 함께 사용하여 충돌 감지 및 물리적 움직임을 처리할 수 있음
        monster = monsterGo.AddComponent<Monster>(); // Monster 스크립트 추가
        return monster;
    }

    void Update()
    {
    }
}
