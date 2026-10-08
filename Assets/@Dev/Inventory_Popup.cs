using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // List를 사용하기 위해 필요

public class Inventory_Popup : UI_Popup
{
    enum Buttons
    {
        IheartButton
    }

    enum Images
    {
        ImageOut
    }

    Transform _content;
    // List<UI_InventoryPopup_Item> _items = new List<UI_InventoryPopup_Item>();

    protected override void Awake()
    {
        base.Awake();
        BindButtons(typeof(Buttons));
        BindImages(typeof(Images));

        GetButton((int)Buttons.IheartButton).onClick.AddListener(() =>
        {
            FindFirstObjectByType<Player>()?.Heal(10);
        });

        // Image는 클릭 이벤트가 없어서 Button을 붙여 사용
        Button outButton = Utils.GetOrAddComponent<Button>(GetImage((int)Images.ImageOut).gameObject);
        outButton.onClick.AddListener(() => UIManager.Instance.ClosePopupUI());
    }   
}