using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_ClickerGame : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText; // 참조값 기입
    int score = 0;
    [SerializeField] Button clickButton; // 참조값 기입

    void Start()
    {
        scoreText.text = "Score: 0";
        clickButton.onClick.AddListener(OnClickButton); // 에디터-코드화
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickButton()
    {
        score++;
        // scoreText.text = $"Score: {score}"; // UI 리프레쉬: 별도로 빼는게 좋음
        RefreshUI();
    }

    public void RefreshUI()
    {
        scoreText.text = $"Score: {score}"; // 별도로 빼주기
    }
}
