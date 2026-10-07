using UnityEngine;

public class UI_DetailPopup : UI_Popup
{
    enum Images
    {
        BG,
    }

    enum Buttons
    {
        CloseButton,
    }

    enum Texts
    {
        CloseButtonText,
    }

    protected override void Awake()
    {
        base.Awake();

        BindImages(typeof(Images));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.CloseButton).onClick.AddListener(() =>
        {
            UIManager.Instance.ClosePopupUI();
        });
    }  
}
