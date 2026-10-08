using UnityEngine;

public class Stage2 : BaseScene
{
    protected override void Awake()
    {
        base.Awake();
        SceneType = Define.EScene.Stage2;

        ResourceManager.Instance.LoadAll();
        UIManager.Instance.ShowSceneUI<UI_DevScene>();
    }
}
