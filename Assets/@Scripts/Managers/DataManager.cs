using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public interface IValidate
{
    bool Validate();
}

public interface IDataLoader<Key, Value> : IValidate
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager : Singleton<DataManager>
{
    private HashSet<IValidate> _loaders = new HashSet<IValidate>(); // 데이터 로더들을 저장하는 HashSet

    public GameConfig GameConfig { get; private set; } // 게임 설정 데이터를 저장하는 속성

    public Dictionary<int, ItemData> ItemDict { get; private set; } = new Dictionary<int, ItemData>(); // 아이템 데이터를 저장하는 딕셔너리

    public void LoadData()
    {
        GameConfig = LoadScriptableObject<GameConfig>("GameConfig");
        ItemDict = LoadJson<ItemDataLoader, int, ItemData>("ItemData").MakeDict();
        // TODO: 추가 데이터 로더를 여기에 로드
        Validate();
    }

    private T LoadScriptableObject<T>(string path) where T : ScriptableObject
    {
        T asset = ResourceManager.Instance.Get<T>(path);
        if (asset == null)
        {
            Debug.LogError($"Failed to load ScriptableObject at path: {path}");
        }
        return asset;
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : IDataLoader<Key, Value>
    {
        TextAsset textAsset = ResourceManager.Instance.Get<TextAsset>("ItemData"); // 예시: "ItemData" JSON 파일 가져오기
        Debug.Log(textAsset.text); // JSON 파일의 내용을 문자열로 가져오기

        Loader loader = JsonConvert.DeserializeObject<Loader>(textAsset.text);
        _loaders.Add(loader);
        Debug.Log(path);
        return loader;

        // foreach (ItemData item in loader.Items)
        // {
        //     Debug.Log($"Item TemplateID: {item.TemplateID}, NameTextID: {item.NameTextID}");
        // }
    }

    private bool Validate() // 모든 데이터 로더의 유효성을 검사하고 결과를 반환
    {
        bool success = true;
        foreach (IValidate loader in _loaders)
        {
            if (loader.Validate() == false)
                success = false;
        }
        _loaders.Clear();
        return success;
    }

}