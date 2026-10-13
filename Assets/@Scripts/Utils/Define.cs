using UnityEngine;

public static class Define
{
    public enum EScene
    {
        Unknown,
        LoadingScene,
        DevScene,
    }

    public enum EEventType
    {
        None,
        GoldChanged,
        LanguageChanged,
        // player 죽음, HP 변경 등 추가 이벤트 가능
    }

    public enum ESound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum ELanguage
    {
        KOR,
        ENG,
    }
}
