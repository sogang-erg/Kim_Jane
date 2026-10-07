using UnityEngine;
using System;

public class GameManager : Singleton<GameManager>
{
    private int _gold = 0;
    public int Gold
    {
        get { return _gold; }
        set
        {
            _gold = value;
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