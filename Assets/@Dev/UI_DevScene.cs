using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_DevScene : UI_Scene

{
    enum Buttons
    {
        InventoryButton,
        GameStartButton,
        GameEndButton,
        HomeButton,
    }

    protected override void Awake()
    {
        base.Awake();

        // 한번에 관리 함수
        // BindImages(typeof(Images));
        BindButtons(typeof(Buttons));
        // BindTexts(typeof(Texts));

        GetButton((int)Buttons.InventoryButton).onClick.AddListener(() =>
        {
            if (UIManager.Instance.GetLastPopupUI<Inventory_Popup>() != null)
                UIManager.Instance.ClosePopupUI();
            else
                UIManager.Instance.ShowPopupUI<Inventory_Popup>();
        });

        GetButton((int)Buttons.GameStartButton).onClick.AddListener(() =>
        {
            FindFirstObjectByType<GameManager>()?.StartGame();
        });

        GetButton((int)Buttons.GameEndButton).onClick.AddListener(() =>
        {
            FindFirstObjectByType<GameManager>()?.GoToNextStage();
        });
        GetButton((int)Buttons.HomeButton).onClick.AddListener(() =>
        {
            FindFirstObjectByType<GameManager>()?.GoToStart();
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
}