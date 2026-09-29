using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameStart;
    [SerializeField] private GameObject gameEnd;
    [SerializeField] private Player player;
    [SerializeField] private Monster monster;
    [SerializeField] private ParticleSystem confetti;

    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;
    private Vector3 monsterStartPosition;
    private Quaternion monsterStartRotation;

    void Start()
    {
        playerStartPosition = player.transform.position;
        playerStartRotation = player.transform.rotation;

        monsterStartPosition = monster.transform.position;
        monsterStartRotation = monster.transform.rotation;

        gameStart.SetActive(true);
        gameEnd.SetActive(false);

        confetti.gameObject.SetActive(false);

        player.enabled = false;
    }

    public void StartGame()
    {
        gameStart.SetActive(false);
        gameEnd.SetActive(false);

        ResetGame();

        player.enabled = true;
    }

    public void EndGame()
    {
        player.enabled = false;

        // Confetti 먼저 활성화
        confetti.gameObject.SetActive(true);

        // 기존 파티클 제거 후 새로 재생
        confetti.Play();

        // 1초 후 End UI
        StartCoroutine(ShowEndUI());
    }

    private IEnumerator ShowEndUI()
    {
        yield return new WaitForSeconds(1.0f);

        gameEnd.SetActive(true);
    }

    void ResetGame()
    {
        player.transform.position = playerStartPosition;
        player.transform.rotation = playerStartRotation;

        monster.transform.position = monsterStartPosition;
        monster.transform.rotation = monsterStartRotation;

        monster.gameObject.SetActive(true);
        monster.ResetMonster();

        // Confetti 완전히 끄기
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

    public void ReplayGame()
    {
        ResetGame();

        gameEnd.SetActive(false);
        gameStart.SetActive(false);

        player.enabled = true;
    }
}