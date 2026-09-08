using UnityEngine;

public class MainScript : MonoBehaviour
{
    public Mesh playerMesh; 
    public Material playerMaterial; 
    public GameObject player;
    
    void Start()
    {
        player = CreatePlayer().gameObject; // gameObject 가져오기
        player.transform.position = new Vector3(10, 10, 10);
        player.GetComponent<Player>().hp = 0;
        GameObject.Destroy(player, 3); // player Script 삭제
        // GameObject.Destroy(player.gameObject, 5); // player 오브제 5초 뒤 삭제
        // player.gameObject.SetActive(false); // 비활성화
        // player.enabled = false; // player Script 비활성화
    }

    Player CreatePlayer()
    {
        GameObject go = new GameObject();
        go.name = "Player";
        go.tag = "Player";

        MeshFilter meshFilter = go.AddComponent<MeshFilter>();
        meshFilter.mesh = playerMesh;
        MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
        meshRenderer.material = playerMaterial;
        Player player = go.AddComponent<Player>();
        player.hp = 200; // 다른 값으로 수정 가능
        // return go;
        return player;
    }

    void Update()
    {
        // Debug.Log(Time.deltaTime);
        if (player != null) // unity fake null = null이 아니라면 
        { 
            // player.hp--;
            player.GetComponent<Player>().hp--; // GameObject용 ~Player script >> fake null 없이는 에러
        }
    }
}
