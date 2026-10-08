using UnityEngine;

public class ObjectBase : MonoBehaviour
{
    public bool Pooling { get; set; } = false;

    public virtual void Awake()
    {
        init(); // Awake 시 초기화-최초 한번은 초기화*
    }

    public virtual void init() // pooling override 초기화 > ObjectManager에서 관리
    {
        
    }
}