using UnityEngine;

public class UI_InventoryPopup_Item : UI_Base // 서브 아이템 개념
{    
    enum Buttons
    {
        UpgradeButton,
    }

    enum Images
    {
    }

    enum Texts
    {
        UpgradeButtonText,
        ItemNameText,
    }

    Transform _content;

    int _templateID;

    protected override void Awake()
    {
        base.Awake();

        BindImages(typeof(Images));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.UpgradeButton).onClick.AddListener(() =>
        {
            Debug.Log("Upgrade button clicked");
        });
    }   

    public void Setinfo(int templateID)
    {
        // templateID를 기반으로 아이템 정보를 설정하는 코드 작성 예정
        _templateID = templateID;
        RefreshUI(); // 아이템 정보를 설정한 후 UI를 갱신

    }

    public void RefreshUI()
    {
        // 아이템 UI를 갱신하는 코드 작성 예정
        GetText((int)Texts.ItemNameText).text = $"Item {_templateID}"; // 예시로 아이템 이름을 templateID 기반으로 설정
        
    }
}
