using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int Gold;
    public int Level;
    public List<int> UpgradeCount;
}

public class GameManager : Singleton<GameManager>
{
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

    public int Gold
    {
        get { return _gameData.Gold; }
        set
        {
            _gameData.Gold = value;
            // 여기서 Gold 값이 변경될 때 추가적인 로직을 수행할 수 있음
            // if (OnGoldChanged != null) // 바뀌면 널리 널리 알리겠다
            //     OnGoldChanged.Invoke();
                // OnGoldChanged.Invoke(value); // 값 알림 버전
            EventManager.Instance.TriggerEvent(Define.EEventType.GoldChanged);
        }
    }

    // 구독 시스템으로 구현-deligate 문법
    // public Action OnGoldChanged; // Gold 값이 변경될 때 호출되는 델리게이트
    // public Action<int> OnGoldChanged; // 얼마가 바뀌었는지 알려주는 델리게이트
    // public event Action OnGoldChanged; // Null이 막힘
}