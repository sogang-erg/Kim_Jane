using UnityEngine;

public class BaseScene : MonoBehaviour
{
    public Define.EScene SceneType {get; protected set;} = Define.EScene.Unknown;

    protected virtual void Awake()
    {
        // TODO: 모든 씬 초기화 공통 로직 작성
    }
}
