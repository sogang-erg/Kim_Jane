using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameStart;
    [SerializeField] private GameObject gameEnd;
    [SerializeField] private GameObject gameOver;
    
    [SerializeField] private Player player;
    [SerializeField] private ParticleSystem confetti;
    [SerializeField] private Transform[] spawnPoints; // 좀비 스폰 위치
    [SerializeField] private int zombieCount = 3;

    private readonly List<Creature> _zombies = new List<Creature>();
    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;

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

        confetti.gameObject.SetActive(false);
        player.enabled = false;
    }

    public void StartGame()
    {
        gameStart.SetActive(false);
        gameEnd.SetActive(false);
        gameOver.SetActive(false);

        ResetGame();

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
        player.enabled = false;
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
        player.enabled = false;

        confetti.gameObject.SetActive(true);
        confetti.Play();

        StartCoroutine(ShowEndUI());
    }

    public void GoToNextStage()
    {
        SceneManager.Instance.LoadNextScene();
    }

    private IEnumerator ShowEndUI()
    {
        yield return new WaitForSeconds(1.0f);
        gameEnd.SetActive(true);
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
