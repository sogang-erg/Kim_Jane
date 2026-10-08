using UnityEngine;

public class Stage3 : BaseScene
{
    protected override void Awake()
    {
        base.Awake();
        SceneType = Define.EScene.Stage3;

        ResourceManager.Instance.LoadAll();
        UIManager.Instance.ShowSceneUI<UI_DevScene>();
    }
}
