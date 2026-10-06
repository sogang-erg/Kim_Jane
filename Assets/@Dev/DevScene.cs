using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class DevScene : BaseScene
{
    protected override void Awake()
    {
        base.Awake();
        SceneType = Define.EScene.DevScene;
        // TODO: 개발 씬 초기화 로직 작성
        // GameObject playerPrefab = ResourceManager.Instance.Get<GameObject>("Player"); // 예시: "Player" 프리팹 가져오기
        ResourceManager.Instance.instantiate("Player"); // 예시: "Player" 프리팹 인스턴스화

        foreach (var item in DataManager.Instance.ItemDict.Values) // 아이템 데이터를 출력
        {
            Debug.Log($"Item TemplateID: {item.TemplateID}, NameTextID: {item.NameTextID}");
        }

        // 게임 설정 데이터를 가져오기
        Debug.Log(DataManager.Instance.GameConfig.InitialGold);
    }
}
