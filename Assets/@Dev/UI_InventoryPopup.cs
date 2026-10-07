using UnityEngine;
using System.Collections.Generic; // List를 사용하기 위해 필요

public class UI_InventoryPopup : UI_Popup
{
    enum GameObjects
    {
        Content,
    }
    enum Images
    {
        BG,
    }

    enum Buttons
    {
        DetailButton,
        CloseButton,
    }

    enum Texts
    {
        DetailButtonText,
        CloseButtonText,
    }

    Transform _content;
    List<UI_InventoryPopup_Item> _items = new List<UI_InventoryPopup_Item>();

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindImages(typeof(Images));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        _content = GetObject((int)GameObjects.Content).transform;

        GetButton((int)Buttons.DetailButton).onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPopupUI<UI_DetailPopup>();
        });

        GetButton((int)Buttons.CloseButton).onClick.AddListener(() =>
        {
            UIManager.Instance.ClosePopupUI();
        });

        SetInfo();
    }   

    public void SetInfo() // 초기화 코드
    {
        _content.DestroyChildren(); // 기존 자식 오브젝트 모두 제거
        for (int i = 0; i < 10; i++)
        {
            UI_InventoryPopup_Item item = UIManager.Instance.ShowUI<UI_InventoryPopup_Item>();
            item.transform.SetParent(_content);

            item.Setinfo(i); // 각 아이템에 templateID를 설정

            _items.Add(item);
        }
    }
}
