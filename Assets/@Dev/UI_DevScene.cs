using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// public class UI_DevScene : MonoBehaviour
// public class UI_DevScene : UI_Base  
public class UI_DevScene : UI_Scene

{
    // 바인딩할 UI 요소의 열거형enum 정의
    enum Buttons
    {
        ClickButton,
    }

    enum Images
    {
        BG,
    }

    enum Texts
    {
        GoldText,
        ClickButtonText,
    }

    // MVC
    // Model대상-View갱신-Controller변화
    // int Gold = 0; // 별도 매니저로 빼는 것이 정석

    // void Awake()
    protected override void Awake()
    {
        base.Awake();

        // Bind<Image>(typeof(Images));
        // Bind<Button>(typeof(Buttons));
        // Bind<TMP_Text>(typeof(Texts));

        // 한번에 관리 함수
        BindImages(typeof(Images));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.ClickButton).onClick.AddListener(OnClickButton);

        // GameManager.Instance.OnGoldChanged += RefreshUI; // 갱신하도록 구독
        // GameManager.Instance.OnGoldChanged -= RefreshUI; // 중복 구독 방지
    }

    private void OnEnable()
    {
        EventManager.Instance.AddEvent(Define.EEventType.GoldChanged, RefreshUI); // EventManager 활용
        // GameManager.Instance.OnGoldChanged += RefreshUI; // 갱신하도록 구독
    }
    
    private void OnDisable()
    {
        EventManager.Instance.RemoveEvent(Define.EEventType.GoldChanged, RefreshUI); // EventManager 활용
        // GameManager.Instance.OnGoldChanged -= RefreshUI; // 구독 해제
    }

    public void OnClickButton() // 클릭 버튼 클릭 시 호출되는 함수-Controller
    {
        GameManager.Instance.Gold++;
        RefreshUI();
    }

    public void RefreshUI() // UI를 갱신하는 함수-View
    // public void RefreshUI(int value) // value는 변경된 Gold 값
    {
        GetText((int)Texts.GoldText).text = $"Gold: {GameManager.Instance.Gold}";
    }
}
