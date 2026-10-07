using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// public class UI_DevScene : MonoBehaviour
// public class UI_DevScene : UI_Base  
public class UI_DevScene : UI_Scene

{
    // 바인딩할 UI 요소의 열거형enum 정의
    enum Images
    {
        BG,
    }

    enum Buttons
    {
        InfoButton,
    }

    enum Texts
    {
        PlayerName,
        InfoButtonText,
    }

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

        GetButton((int)Buttons.InfoButton).onClick.AddListener(() =>
        {
            // Debug.Log("InfoButton clicked");
            // GetText((int)Texts.PlayerName).text = "Clicked!";
            UIManager.Instance.ShowPopupUI<UI_InventoryPopup>();
        });
    }

    // // 인스펙터에 표시되도록 설정 // 수동 드래그앤드롭
    // [SerializeField]
    // private Button _infoButton;
    // [SerializeField] 
    // private TMP_Text _infoButtonText;
    // [SerializeField]
    // private Image _backgroundImage; 
    // [SerializeField] 
    // private TMP_Text _playerNameText;

    // public Button _infoButton;
    // public TMP_Text _infoButtonText;
    // public Image _backgroundImage; 
    // public TMP_Text _playerNameText;

    void Start()
    {
        // // 소스 코드를 통해 자식 오브젝트에서 컴포넌트를 찾아 할당 (코드 기반 할당)
        // // _backgroundImage = Utils.FindChildGameObject(gameObject, "BG", true).GetComponent<Image>(); // 자식에 붙어있는 게임오브젝트 찾기
        // _backgroundImage = Utils.FindChildComponent<Image>(gameObject, "BG", true); // 자식에 붙어있는 컴포넌트 찾기
        // _infoButton = Utils.FindChildComponent<Button>(gameObject, "InfoButton", true); 
        // _infoButtonText = Utils.FindChildComponent<TMP_Text>(gameObject, "InfoButtonText", true);
        // _playerNameText = Utils.FindChildComponent<TMP_Text>(gameObject, "PlayerName", true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
