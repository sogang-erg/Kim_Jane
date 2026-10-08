using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour // 안심
{
    static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>(); // SceneManager이 씬에 존재하는지 확인
                if (_instance == null)
                {
                    GameObject go = new GameObject($"@{typeof(T).Name}");
                    _instance = go.AddComponent<T>();
                }
                DontDestroyOnLoad(_instance.gameObject); // 씬 전환에도 인스턴스 유지
            }
            return _instance;
        }
    }
}