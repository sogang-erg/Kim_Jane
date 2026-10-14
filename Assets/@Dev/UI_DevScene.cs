using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;

public class UI_DevScene : UI_Scene

{
    enum Buttons
    {
        InventoryButton,
        GameStartButton,
        GameEndButton,
        HomeButton,
    }

    enum Texts
    {
        LevelText,
    }

    enum Images
    {
        CongratsImage,
    }

    protected override void Awake()
    {
        base.Awake();

        // 한번에 관리 함수
        BindImages(typeof(Images));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        UpdateLevelText(1);

        GetButton((int)Buttons.InventoryButton).onClick.AddListener(() =>
        {
            SoundManager.Instance.Play2D(Define.ESound.Button, "button");

            if (UIManager.Instance.GetLastPopupUI<Inventory_Popup>() != null)
                UIManager.Instance.ClosePopupUI();
            else
                UIManager.Instance.ShowPopupUI<Inventory_Popup>();
        });

        GetButton((int)Buttons.GameStartButton).onClick.AddListener(() =>
        {
            SoundManager.Instance.Play2D(Define.ESound.Button, "button");
            FindFirstObjectByType<GameManager>()?.StartGame();
        });

        GetButton((int)Buttons.GameEndButton).onClick.AddListener(() =>
        {
            SoundManager.Instance.Play2D(Define.ESound.Button, "button");
            FindFirstObjectByType<GameManager>()?.GoToNextStage();
        });
        GetButton((int)Buttons.HomeButton).onClick.AddListener(() =>
        {
            SoundManager.Instance.Play2D(Define.ESound.Button, "button");
            FindFirstObjectByType<GameManager>()?.GoToStart();
            SoundManager.Instance.Play2D(Define.ESound.Button, "button");
        });

        // GetButton((int)Buttons.ReplayButton).onClick.AddListener(() =>
        // {
        //     FindFirstObjectByType<GameManager>()?.ReplayGame();
        // });
    }

    public GameObject GameStartPanel =>
    GetButton((int)Buttons.GameStartButton).transform.parent.gameObject;

    public GameObject GameEndPanel =>
    GetButton((int)Buttons.GameEndButton).transform.parent.gameObject;

    public GameObject GameOverPanel =>
    GetButton((int)Buttons.HomeButton).transform.parent.gameObject;

    public GameObject CongratsPanel =>
    GetImage((int)Images.CongratsImage).transform.parent.gameObject;

    public void UpdateLevelText(int level)
    {
        // GetText((int)Texts.LevelText).text = $"{level}";
        TMP_Text text = GetText((int)Texts.LevelText);
        if (text == null)
        {
            Debug.LogError("LevelText를 찾지 못했습니다.");
            return;
        }
        text.text = $"{level}";
    }
}