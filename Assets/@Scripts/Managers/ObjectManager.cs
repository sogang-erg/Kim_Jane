using UnityEngine;
using System.Collections.Generic;

public class ObjectManager : Singleton<ObjectManager>
{
    #region Roots 
    private Transform _playerRoot;
    public Transform PlayerRoot
    {
        // get
        // {
        //     if (_playerRoot == null)
        //     {
        //         GameObject go = new GameObject("PlayerRoot");
        //         _playerRoot = go.transform;
        //     }
        //     return _playerRoot;
        // }
        get { return Utils.GetRootTransform(ref _playerRoot, "@Players"); }
    }

    private Transform _monsterRoot;
    public Transform MonsterRoot
    {
        get { return Utils.GetRootTransform(ref _monsterRoot, "@Monsters"); }
    }

    private Transform _npcRoot;
    public Transform NpcRoot
    {
        get { return Utils.GetRootTransform(ref _npcRoot, "@Npcs"); }
    }
    #endregion

    // HashSet 활용, 이중 관리용 HashSet (통합 + 개별)
    // 통합용
    // private HashSet<GameObject> _objects = new HashSet<GameObject>();
    private HashSet<ObjectBase> _objects = new HashSet<ObjectBase>();
    // 개별 객체용
    private HashSet<Player> _players = new HashSet<Player>();
    private HashSet<Monster> _monsters = new HashSet<Monster>();
    private HashSet<Npc> _npcs = new HashSet<Npc>();
    // 온란인 게임 경우 물체-식별 정수ID = 딕션너리로 관리된다

    // public Player SpawnPlayer(string prefab = "Player") // 프리팹 인자화
    public Player SpawnPlayer(string prefab = "Player", bool pooling = false) 
    {
        // GameObject go = ResourceManager.Instance.Instantiate(prefab);
        GameObject go = null;
        if (pooling)
            go = PoolManager.Instance.Pop(prefab);
        else
            go = ResourceManager.Instance.Instantiate(prefab);

        go.name = prefab;
        go.transform.parent = PlayerRoot; // PlayerRoot에 부모 설정

        Player player = go.GetOrAddComponent<Player>();
        _objects.Add(player);
        _players.Add(player);

        player.Pooling = pooling; // 풀링 여부 설정

        return player;
    }

    public Monster SpawnMonster(string prefab = "Monster", bool pooling = false) 
    {
        // GameObject go = ResourceManager.Instance.Instantiate(prefab);
        GameObject go = null;
        if (pooling)
            go = PoolManager.Instance.Pop(prefab);
        else
            go = ResourceManager.Instance.Instantiate(prefab);

        go.name = prefab;
        go.transform.parent = MonsterRoot;

        Monster monster = go.GetOrAddComponent<Monster>();
        _objects.Add(monster);
        _monsters.Add(monster);

        monster.Pooling = pooling; // 풀링 여부 설정

        return monster;
    }

    public Npc SpawnNpc(string prefab = "Npc", bool pooling = false) 
    {
        // GameObject go = ResourceManager.Instance.Instantiate(prefab);
        GameObject go = null;
        if (pooling)
            go = PoolManager.Instance.Pop(prefab);
        else
            go = ResourceManager.Instance.Instantiate(prefab);

        go.name = prefab;
        go.transform.parent = NpcRoot;

        Npc npc = go.GetOrAddComponent<Npc>();
        _objects.Add(npc);
        _npcs.Add(npc);

        npc.Pooling = pooling; // 풀링 여부 설정

        return npc;
    }

    public void Despawn(ObjectBase obj)
    {
        if (obj == null)
            return;

        _objects.Remove(obj);

        if (obj is Player player)
            _players.Remove(player);
        else if (obj is Monster monster)
            _monsters.Remove(monster);
        else if (obj is Npc npc)
            _npcs.Remove(npc);

        if (obj.Pooling)
        {
            obj.init(); // 풀링된 객체 초기화
            PoolManager.Instance.Push(obj.gameObject);
        }
        else
            ResourceManager.Instance.Destroy(obj.gameObject);
    }
}
