using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour // 안심
{
    static T _instance;
    static bool _init = false; // 초기화 여부 확인용

    public static T Instance
    {
        get
        {
            if (_instance == null && _init == false)
            // fake null check for Unity objects
            // GameObject만 소멸하더라도 null로 인지가 되는 것 예방
            // 초기화를 했으면 더 이상 FindFirstObjectByType를 호출하지 않음
            {
                _instance = FindFirstObjectByType<T>(); // SceneManager이 씬에 존재하는지 확인
                _init = true; // 초기화 완료 표시

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
