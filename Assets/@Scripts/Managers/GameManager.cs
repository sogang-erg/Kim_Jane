using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameData
{
    public int EXP;
    public int Level;
    public List<int> UpgradeCount;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameStart;
    [SerializeField] private GameObject gameEnd;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject congrats;
    
    [SerializeField] private Player player;
    [SerializeField] private ParticleSystem confetti;
    [SerializeField] private Transform[] spawnPoints; // 좀비 스폰 위치
    [SerializeField] private int zombieCount = 3;

    private readonly List<Creature> _zombies = new List<Creature>();
    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameData _gameData = new GameData();

    public GameData GameData // 게임 데이터 클래스로 모아 관리
    {
        get { return _gameData; }
        set
        {
            _gameData = value;
        }
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        playerStartPosition = player.transform.position;
        playerStartRotation = player.transform.rotation;

        gameStart = FindFirstObjectByType<UI_DevScene>().GameStartPanel;
        gameStart.SetActive(true);
        gameEnd = FindFirstObjectByType<UI_DevScene>().GameEndPanel;
        gameEnd.SetActive(false);
        gameOver = FindFirstObjectByType<UI_DevScene>().GameOverPanel;
        gameOver.SetActive(false);
        congrats = FindFirstObjectByType<UI_DevScene>().CongratsPanel;
        congrats.SetActive(false);

        confetti.gameObject.SetActive(false);
        player.enabled = false;
    }

    public void StartGame()
    {
        gameStart.SetActive(false);
        gameEnd.SetActive(false);
        gameOver.SetActive(false);
        congrats.SetActive(false);

        // 리소스를 먼저 로드
        ResourceManager.Instance.LoadAll();
        // GameConfig 로드
        DataManager.Instance.LoadData();

        // BGM 볼륨을 30%로 설정
        SoundManager.Instance.SetSoundVolume(Define.ESound.Bgm, 0.1f);
        if (!SoundManager.Instance.IsBgmPlaying())
        SoundManager.Instance.Play2D(Define.ESound.Bgm, "bgm");

        // 저장 데이터가 있으면 불러오기
        // 저장 데이터가 없으면 GameConfig의 초기값 사용
        SaveManager.Instance.Load();
        SaveManager.Instance.StartAutoSave();

        Debug.Log($"게임 시작 전 EXP: {GameData.EXP}, Level: {GameData.Level}");

        // // GameConfig의 초기값을 GameData에 설정
        // GameData.EXP = DataManager.Instance.GameConfig.InitialEXP;
        // GameData.Level = DataManager.Instance.GameConfig.InitialLevel;

        ResetGame();

        // 초기 레벨을 UI에 반영
        UI_DevScene ui = FindFirstObjectByType<UI_DevScene>();
        if (ui != null)
            ui.UpdateLevelText(GameData.Level);
        player.enabled = true;
    }

    private Creature SpawnZombieByIndex(int index)
    {
        switch (index % 3)
        {
            case 0:  return ObjectManager.Instance.SpawnZombie("Zombie", pooling: true);
            case 1:  return ObjectManager.Instance.SpawnZombie1("Zombie1", pooling: true);
            default: return ObjectManager.Instance.SpawnZombie2("Zombie2", pooling: true);
        }
    }

    public void OnPlayerDied()
    {
        Debug.Log("플레이어 사망 처리 실행!");

        player.enabled = false;
        // BGM 정지
        SoundManager.Instance.StopBgm();
        // 게임 오버 효과음 재생
        SoundManager.Instance.Play2D(Define.ESound.Effect, "over");

        gameOver.SetActive(true);
    }

    public void OnZombieDied(Creature zombie)
    {
        _zombies.Remove(zombie);

        if (_zombies.Count == 0)
            EndGame(); // 모든 좀비 처치
    }

    public void EndGame()
    {
        Debug.Log("게임 클리어 처리 실행!");

        player.enabled = false;

        confetti.gameObject.SetActive(true);
        confetti.Play();

        // 폭죽 효과음 재생
        // SoundManager.Instance.SetSoundVolume(Define.ESound.Effect, 0.5f);
        SoundManager.Instance.Play2D(Define.ESound.Effect, "confetti");

        StartCoroutine(ShowEndUI());
    }

    public void GoToNextStage()
    {
        Debug.Log($"스테이지 이동 전 EXP: {GameData.EXP}, Level: {GameData.Level}");
        // 씬 전환 중 자동 저장 중지
        SaveManager.Instance.StopAutoSave();
        // 현재 경험치와 레벨 저장
        SaveManager.Instance.Save();
        // 다음 스테이지로 이동
        SceneManager.Instance.LoadNextScene();
    }

    private IEnumerator ShowEndUI()
    {
        yield return new WaitForSeconds(1.0f);

        if (SceneManager.Instance.IsLastStage())
        {
            // 게임 오버 음악 정지
            SoundManager.Instance.StopBgm();
            SoundManager.Instance.Play2D(Define.ESound.Effect, "clear");
            congrats.SetActive(true);
        }
        else
        {
            gameEnd.SetActive(true);
        }

        Debug.Log($"Congrats 활성화: {congrats.activeSelf}");
        Debug.Log($"GameEnd 활성화: {gameEnd.name}");
        Debug.Log($"GameOver 활성화 상태: {gameOver.activeSelf}");
    }

    void ResetGame()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("GameManager: Spawn Points를 연결하세요.");
            return;
        }

        player.transform.SetPositionAndRotation(playerStartPosition, playerStartRotation);
        player.ResetHealth();
        gameOver.SetActive(false);

        foreach (Creature z in _zombies)
            ObjectManager.Instance.Despawn(z);
        _zombies.Clear();

        for (int i = 0; i < zombieCount; i++)
        {
            Creature z = SpawnZombieByIndex(i);
            Transform sp = spawnPoints[i % spawnPoints.Length];
            z.Place(sp.position, sp.rotation);
            _zombies.Add(z);
        }

        confetti.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        confetti.gameObject.SetActive(false);
    }
    public void GoToStart()
    {
        ResetGame();

        // 게임 오버 음악 정지
        SoundManager.Instance.StopOver();

        gameEnd.SetActive(false);
        gameStart.SetActive(true);

        player.enabled = false;
    }
    // public void ReplayGame()
    // {
    //     ResetGame();

    //     gameEnd.SetActive(false);
    //     gameStart.SetActive(false);

    //     player.enabled = true;
    // }
}
