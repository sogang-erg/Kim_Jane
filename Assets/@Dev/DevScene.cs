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
        ResourceManager.Instance.LoadAll();

        // UI
        UIManager.Instance.ShowSceneUI<UI_DevScene>(); // 개발 씬 UI 표시, UI_DevScene이 프리팻화되어 있어야 함


    }
}
