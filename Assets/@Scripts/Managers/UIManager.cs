using UnityEngine;
using System.Collections.Generic; // Stack와 Dictionary를 사용하기 위해 필요

public class UIManager : Singleton<UIManager>
{
    Transform _root; // 모든 UI 오브젝트의 부모-GameObject
    Transform Root 
    {
        get
        {
            if (_root == null)
                _root = new GameObject("@UI_Root").transform; // Transform 활용
            return _root;
        }
    }

    #region Scene UI
    private UI_Scene _sceneUI;
    public UI_Scene SceneUI
    {
        get
        {
            if (_sceneUI)
                _sceneUI = FindFirstObjectByType<UI_Scene>(); // UI_Scene 컴포넌트를 찾아 할당
            return _sceneUI;
        }
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (_sceneUI != null)
            return _sceneUI as T;

        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        T sceneUI = FindFirstObjectByType<T>();
        if (sceneUI == null)
        {
            GameObject go = ResourceManager.Instance.Instantiate(name);
            sceneUI = Utils.GetOrAddComponent<T>(go);
        }

        sceneUI.transform.SetParent(Root); // UI 오브젝트를 루트 아래에 배치
        _sceneUI = sceneUI;

        return sceneUI;
    }
    #endregion

    #region Popup UI
    Transform _popupRoot;
    Transform PopupRoot
    {
        get
        {
            if (_popupRoot == null)
            {
                GameObject go = new GameObject("@PopupRoot");
                // _popupRoot = go.transform; // Transform 활용
                // _popupRoot.SetParent(Root); // PopupRoot를 UI_Root 아래에 배치
                go.transform.SetParent(Root); // PopupRoot를 UI_Root 아래에 배치
                _popupRoot = go.transform; // Transform 활용
            }
            return _popupRoot;
        }
    }

    private int _popupOrder = 100; // 팝업 UI의 정렬 순서 시작 값
    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    private Dictionary<string, UI_Popup> _popups = new Dictionary<string, UI_Popup>();

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        if (_popups.TryGetValue(name, out UI_Popup popup) == false)
        {
            GameObject go = ResourceManager.Instance.Instantiate(name);
            popup = Utils.GetOrAddComponent<T>(go);
            _popups[name] = popup;
        }

        _popupStack.Push(popup); // 팝업 UI를 스택에 추가

        popup.transform.SetParent(PopupRoot); // 팝업 UI를 PopupRoot 아래에 배치
        popup.gameObject.SetActive(true); // 활성화
        _popupOrder++;
        popup.GetComponent<Canvas>().sortingOrder = _popupOrder; // 팝업 UI의 정렬 순서를 설정

        return popup as T;
    }

    public T GetLastPopupUI<T>() where T : UI_Popup // 스택에서 가장 최근에 추가된 팝업 UI를 반환
    {
        if (_popupStack.Count == 0)
            return null;

        return _popupStack.Peek() as T; // 스택의 최상단에 있는 팝업 UI를 반환
    }

    public void ClosePopupUI() // 가장 최근에 추가된 팝업 UI를 닫음
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        popup.gameObject.SetActive(false); // 비활성화
        _popupOrder--; // 팝업 UI의 정렬 순서를 감소시킴 (닫힌 팝업이 있으면 순서를 낮춤)
    }

    public void CloseAllPopupUI() // 모든 팝업 UI를 닫음
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    } 
    #endregion

    public T ShowUI<T>(string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourceManager.Instance.Instantiate(name);

        return go.GetOrAddComponent<T>();
    }

    public void Clear() // 언제든지 UI Manager를 초기화할 때 사용
    {
        CloseAllPopupUI();
        _popups.Clear();

        Root.DestroyChildren();

        _sceneUI = null;
    }

}
