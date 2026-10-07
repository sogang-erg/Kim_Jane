using UnityEngine;
using System;
using System.Collections.Generic;
public interface IResourceLoader 
{
    void LoadAll(Action<float> onProgress = null, Action onComplete = null); // 모든 리소스를 로드, 진행상태를 콜백으로 전달
    T Get<T>(string key) where T : UnityEngine.Object; // 키를 통해 리소스를 가져옴
    GameObject Instantiate(string key, Transform parent = null); // 인스턴스화
    void ReleaseAll();

}

public class ResourceManager : Singleton<ResourceManager>
{
    IResourceLoader _loader = new ResourcesLoader();
    // 교체 할 수 있는 방안 마련

    public void LoadAll(Action<float> onProgress = null, Action onComplete = null)
    {
        _loader.LoadAll(onProgress, onComplete);
    }

    public T Get<T>(string key) where T : UnityEngine.Object
    {
        return _loader.Get<T>(key);
    }

    public GameObject Instantiate(string key, Transform parent = null)
    {
        return _loader.Instantiate(key, parent);
    }

    public void ReleaseAll()
    {
        _loader.ReleaseAll();
    }
}

public class ResourcesLoader : IResourceLoader
{
    private Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>(); // 키-리소스 쌍을 저장하는 딕셔너리

    public void LoadAll(Action<float> onProgress = null, Action onComplete = null)
    {
        List<string> paths = new List<string>{ "Prefabs", "Config", "Data/JasonData" }; // 로드할 리소스 경로 목록
        int totalPath = paths.Count;
        int loadedPath = 0;

        foreach (string path in paths)
        {
            // TODO: Load All  
            UnityEngine.Object[] resources = Resources.LoadAll(path);
            foreach (UnityEngine.Object resource in resources)
            {
                string fullKey = $"{resource.name}_{resource.GetType().Name}";
                if (_resources.ContainsKey(fullKey) == false)
                    _resources[fullKey] = resource;
            }
            loadedPath++;
            float progress = (float)loadedPath / totalPath;
            onProgress?.Invoke(progress);

            if (loadedPath >= totalPath)
                onComplete?.Invoke();
        }
    }

    public T Get<T>(string key) where T : UnityEngine.Object
    {
        string fullKey = $"{key}_{typeof(T).Name}";
        if (_resources.TryGetValue(fullKey, out UnityEngine.Object resource))
            return resource as T;
        return null;
    }

    public GameObject Instantiate(string key, Transform parent = null)
    {
        GameObject prefab = Get<GameObject>(key);
        if (prefab == null)
            return null;
        GameObject instance = GameObject.Instantiate(prefab, parent);
        instance.name = prefab.name;
        return instance;
    }

    public void ReleaseAll() // 모든 로드된 리소스를 해제-큰 게임에서나 발생
    {
        foreach (UnityEngine.Object resource in _resources.Values)
            Resources.UnloadAsset(resource);

        _resources.Clear();
        Resources.UnloadUnusedAssets();
    }
}
