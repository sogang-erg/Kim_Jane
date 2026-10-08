using UnityEngine;
using System.Collections.Generic;

public class Stage1 : BaseScene
{
    protected override void Awake()
    {
        base.Awake();
        SceneType = Define.EScene.Stage1;

        ResourceManager.Instance.LoadAll();
        UIManager.Instance.ShowSceneUI<UI_DevScene>();
    }
}