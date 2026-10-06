using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Game Settings")] // 헤더에 게임 설정 섹션을 표시

    [Min(500)] // 최솟값 설정
    [SerializeField] // 인스펙터에 표시되도록 설정
    private int initialGold = 1000;
    
    [Range(1, 20)] // 슬라이더 생성
    [SerializeField] // 인스펙터에 표시되도록 설정
    private int initialLevel = 1;

    public int InitialGold => initialGold; // 초기 골드 값을 외부에서 읽기 전용으로 접근 가능
    public int InitialLevel => initialLevel; // 초기 레벨 값을 외부에서 읽기 전용으로 접근 가능

    public GameObject prefab; // 프리팹 연결 가능
}
