using UnityEngine;

public static class Utils
{
    // GameObject에 해당 컴포넌트가 없으면 추가하고 반환하는 유틸리티 메서드
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        return component;
    }

    // GameObject의 자식 중 이름이 일치하는 오브젝트를 검색하는 유틸리티 메서드
    // recursive가 true이면 모든 하위 계층을 검색, false이면 직계 자식만 검색
    public static GameObject FindChildGameObject(GameObject go, string name, bool recursive = false)
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            // 직계 자식만 검색
            // 직계 자식만 검색하는 기존 방식 (foreach 사용)
            // foreach (Transform child in go.transform)
            // {
            //     if (child.name == name)
            //         return child.gameObject;
            // }

            // for 루프를 사용하여 직계 자식만 검색
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform child = go.transform.GetChild(i);
                if (child.name == name)
                    return child.gameObject;
            }
        }
        else
        {
            // 모든 하위 계층 검색 (비활성화된 오브젝트 포함)
            Transform[] children = go.GetComponentsInChildren<Transform>(includeInactive: true);
            foreach (Transform child in children)
            {
                if (child.name == name)
                    return child.gameObject;
            }
        }

        return null;
    }

    // GameObject의 자식 중 이름이 일치하는 컴포넌트를 검색하는 유틸리티 메서드
    // recursive가 true이면 모든 하위 계층을 검색, false이면 직계 자식만 검색
    public static T FindChildComponent<T>(GameObject go, string name = null, bool recursive = false) where T : Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            // 직계 자식만 검색
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform child = go.transform.GetChild(i);

                if (string.IsNullOrEmpty(name) || child.name == name)
                {
                    T component = child.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            // 모든 하위 계층 검색 (비활성화된 오브젝트 포함)
            T[] children = go.GetComponentsInChildren<T>(includeInactive: true);
            foreach (T child in children)
            {
                if (string.IsNullOrEmpty(name) || child.name == name)
                    return child;
            }
        }

        return null;
    }

    public static Transform GetRootTransform(ref Transform root, string name, Transform parent = null)
    {
        if (root == null)
        {
            GameObject go = GameObject.Find(name);
            if (go == null)
                go = new GameObject(name);

            root = go.transform;
            root.SetParent(parent);
        }

        return root;
    }

}