using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Random = UnityEngine.Random;

public class DevScene : BaseScene
{
    protected override void Awake()
    {
        base.Awake();
        SceneType = Define.EScene.DevScene;
        // TODO: 개발 씬 초기화 로직 작성
        ResourceManager.Instance.LoadAll();

        // UI
        // UIManager.Instance.ShowSceneUI<UI_DevScene>(); // 개발 씬 UI 표시, UI_DevScene이 프리팻화되어 있어야 함

        // GameManager.Instance.OnGoldChanged += () =>
        // {
        //     // 구독 신청 가능-chain in 기능
        // };

        // // 미리 스택 쌓기 예시
        // GameManager.Instance.Gold++; // 예시로 Gold 값을 증가시켜 구독자에게 알림
        // GameManager.Instance.Gold++;
        // GameManager.Instance.Gold++;

        // ResourceManager.Instance.Instantiate("Player");
        // ResourceManager.Instance.Instantiate("Monster");
        // ResourceManager.Instance.Instantiate("Npc");

        // ObjectManager를 통해 플레이어, 몬스터, NPC를 스폰
        // for (int i = 0; i < 5; i++)
        //     ObjectManager.Instance.SpawnPlayer();

        // for (int i = 0; i < 20; i++)
        //     ObjectManager.Instance.SpawnMonster(); 

        // for (int i = 0; i < 10; i++)
        //     ObjectManager.Instance.SpawnNpc();

        // // 랜덤 위치 스폰-방법 #1
        // for (int i = 0; i < 10; i++)
        // {
        //     ObjectManager.Instance.SpawnPlayer().transform.position = RandomSpawnPosition();
        //     ObjectManager.Instance.SpawnMonster().transform.position = RandomSpawnPosition();
        //     ObjectManager.Instance.SpawnNpc().transform.position = RandomSpawnPosition();
        // }

        // // 랜덤 위치 스폰-방법 #2
        // for (int i = 0; i < 10; i++)
        // {
        //     Player player = ObjectManager.Instance.SpawnPlayer();
        //     player.transform.position = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
        //     Monster monster = ObjectManager.Instance.SpawnMonster();
        //     monster.transform.position = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
        //     Npc npc = ObjectManager.Instance.SpawnNpc();
        //     npc.transform.position = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
        // }

        // PoolManager.Instance.Reserve("Monster", 50);
        List<Monster> monsters = new List<Monster>();
        
        for (int i = 0; i < 10; i++)
        {
            // PoolManager.Instance.Pop("Monster"); // Pool에서 몬스터를 꺼냄
            // GameObject monster = PoolManager.Instance.Pop("Monster");
            // PoolManager.Instance.Push(monster); // Pool에 몬스터를 다시 넣음
            // // 결과적으로 몬스터를 꺼내고 다시 풀에 넣는 과정을 반복함-몬스터 객체가 계속 재사용됨

            Monster monster = ObjectManager.Instance.SpawnMonster("Monster", pooling: true); // 디폴트 풀링 사용-풀링을 통해 몬스터 스폰
            monsters.Add(monster);
        }

        for (int i = 0; i < 10; i++)
        {
            ObjectManager.Instance.Despawn(monsters[i]); // 풀링된 몬스터를 다시 풀에 반환
        }
    
    }

    // // 랜덤 위치 스폰-방법 #1
    // private Vector3 RandomSpawnPosition()
    // {
    //     return new Vector3(
    //         UnityEngine.Random.Range(-10f, 10f),
    //         0f,
    //         UnityEngine.Random.Range(-10f, 10f));
    // }

}
