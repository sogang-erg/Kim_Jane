using UnityEngine;

public class SceneManager : Singleton<SceneManager>
{
    private BaseScene _currentScene; // 현재 활성화된 씬의 BaseScene 참조
    public BaseScene CurrentScene
    {
        get
        {
            if (_currentScene == null)
                _currentScene = FindFirstObjectByType<BaseScene>();
            return _currentScene;
        }
    }

    public Define.EScene CurrentSceneType // 현재 활성화된 씬의 타입을 반환
    {
        get
        {
            if (CurrentScene == null)
                return Define.EScene.DevScene;
            return CurrentScene.SceneType;
        }
    }

    public void LoadScene(Define.EScene sceneType) // 지정한 씬을 로드
    {
        string sceneName = sceneType.ToString();
        // 씨 전환 시 풀에 남은 오브젝트는 파괴되므로 풀을 비움
        PoolManager.Instance.Clear();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        _currentScene = null; // 씬을 로드한 후 현재 씬 참조 초기화
    }

    public void LoadNextScene()
    {
        Define.EScene next = CurrentSceneType + 1;

        if (!System.Enum.IsDefined(typeof(Define.EScene), next))
        {
            return;
        }

        LoadScene(next);
    }
}