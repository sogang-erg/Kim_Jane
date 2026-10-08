using UnityEngine;

public class BaseScene : MonoBehaviour
{
    public Define.EScene SceneType {get; protected set;} = Define.EScene.
    DevScene;

    protected virtual void Awake()
    {
        // TODO: 모든 씬 초기화 공통 로직 작성
    }
}
