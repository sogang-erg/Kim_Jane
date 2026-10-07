using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, Object[]> _objects = new Dictionary<Type, Object[]>();

    protected virtual void Awake()
    {
        // 초기화 시 필요한 작업이 있으면 여기에 작성
        if (Object.FindAnyObjectByType<EventSystem>() == null)
        {
            ResourceManager.Instance.Instantiate("EventSystem"); // EventSystem 프리팹을 인스턴스화
        }
    }

    protected void BindObjects(Type type) { Bind<GameObject>(type); }
    protected void BindImages(Type type) { Bind<Image>(type); }
    protected void BindTexts(Type type) { Bind<TMP_Text>(type); }
    protected void BindButtons(Type type) { Bind<Button>(type); }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected TMP_Text GetText(int idx) { return Get<TMP_Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    protected void Bind<T>(Type type) where T : Object // Binding
    {
        string[] names = Enum.GetNames(type);
        Object[] objects = new Object[names.Length];

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Utils.FindChildGameObject(gameObject, names[i], true);
            else
                objects[i] = Utils.FindChildComponent<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }

        _objects.Add(typeof(T), objects); // 최종적으로 바인딩된 오브젝트들을 딕셔너리에 추가
    }

    protected T Get<T>(int idx) where T : Object // 타입의 바인딩된 오브젝트를 인덱스로 가져오기
    {
        if (_objects.TryGetValue(typeof(T), out Object[] objects) == false)
            return null;

        return objects[idx] as T;
    }
}
