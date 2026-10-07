using UnityEngine;

public static class Extension
{
    // GameObject에 해당 컴포넌트가 없으면 추가하고 반환하는 확장 메서드-this로 함수 추가
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
       return Utils.GetOrAddComponent<T>(go);
    }

    // GameObject의 자식 오브젝트를 모두 삭제하는 확장 메서드-this로 함수 추가
    public static void DestroyChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    
    // GameObject의 자식 오브젝트를 모두 삭제하는 확장 메서드-this로 함수 추가 (GameObject용)
    public static void DestroyChildren(this GameObject go)
    {
        go.transform.DestroyChildren();
    }


}
