using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : BaseScene
{
    protected override void Awake()
    {
        base.Awake(); 
        SceneType = Define.EScene.LoadingScene;
        // TODO: 로딩 씬 초기화 로직 작성
        // SceneManager.LoadScene("DevScene");
        // UnityEngine.SceneManagement.SceneManager.LoadScene("DevScene"); // 명시적으로 네임스페이스를 사용하여 씬 로드

        // Resources.Load("Prefabs/Player"); // 동기적으로 로드
        // Resources.LoadAll("Prefabs"); // 모든 리소스를 동기적으로 로드
        // Resources.LoadAsync("Prefabs/Player"); // 비동기적으로 로드

        ResourceManager.Instance.LoadAll(OnProgress, OnComplete);
        SceneManager.Instance.LoadScene(Define.EScene.DevScene); // Manager class를 통해 씬 로드
    }

    // void LoadScene() // 랩핑 방법
    // {
    //     // TODO: 씬 로딩 로직 작성
    //     SceneManager.LoadScene("DevScene");
    // }

    void OnProgress(float value)
    {
       Debug.Log($"Loading progress: {value * 100}%");
    }

    void OnComplete()   
    {
       Debug.Log("Loading complete.");
       DataManager.Instance.LoadData(); // 데이터 매니저를 통해 데이터 로드
       SceneManager.Instance.LoadScene(Define.EScene.DevScene); // 로딩 완료 후 DevScene으로 전환
    }
}
