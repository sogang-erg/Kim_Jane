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
                return Define.EScene.Unknown;
            return CurrentScene.SceneType;
        }
    }

    public void LoadScene(Define.EScene sceneType) // 지정한 씬을 로드
    {
        string sceneName = sceneType.ToString();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        _currentScene = null; // 씬을 로드한 후 현재 씬 참조 초기화
    }
}
