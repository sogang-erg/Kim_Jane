using UnityEngine;

public class UI_Localization : UI_Base
{
    enum Buttons
    {
        KoreanButton,
        EnglishButton,
    }

    enum Texts
    {
        MessageText,
    }

    protected override void Awake()
    {
        base.Awake();
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.KoreanButton).onClick.AddListener(() =>
        {
            LocalizationManager.Instance.CurrentLanguage = Define.ELanguage.KOR;
        });

        GetButton((int)Buttons.EnglishButton).onClick.AddListener(() =>
        {
            LocalizationManager.Instance.CurrentLanguage = Define.ELanguage.ENG;
        });
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        GetText((int)Texts.MessageText).SetLocalizedText("HELLO"); // 메시지 텍스트를 로컬라이즈된 "Hello"로 설정
    }
}
