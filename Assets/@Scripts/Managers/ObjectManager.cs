using UnityEngine;
using System.Collections.Generic;

public class ObjectManager : Singleton<ObjectManager>
{
    #region Roots
    private Transform _zombieRoot;
    public Transform ZombieRoot
    {
        get { return Utils.GetRootTransform(ref _zombieRoot, "@Zombies"); }
    }

    private Transform _zombie1Root;
    public Transform Zombie1Root
    {
        get { return Utils.GetRootTransform(ref _zombie1Root, "@Zombie1s"); }
    }

    private Transform _zombie2Root;
    public Transform Zombie2Root
    {
        get { return Utils.GetRootTransform(ref _zombie2Root, "@Zombie2s"); }
    }
    #endregion

    // 통합용 + 개별용 이중 관리
    private HashSet<ObjectBase> _objects = new HashSet<ObjectBase>();
    private HashSet<Zombie> _zombies = new HashSet<Zombie>();
    private HashSet<Zombie1> _zombie1s = new HashSet<Zombie1>();
    private HashSet<Zombie2> _zombie2s = new HashSet<Zombie2>();

    public Zombie SpawnZombie(string prefab = "Zombie", bool pooling = false)
    {
        Zombie zombie = Spawn<Zombie>(prefab, ZombieRoot, pooling);
        _zombies.Add(zombie);
        return zombie;
    }

    public Zombie1 SpawnZombie1(string prefab = "Zombie1", bool pooling = false)
    {
        Zombie1 zombie = Spawn<Zombie1>(prefab, Zombie1Root, pooling);
        _zombie1s.Add(zombie);
        return zombie;
    }

    public Zombie2 SpawnZombie2(string prefab = "Zombie2", bool pooling = false)
    {
        Zombie2 zombie = Spawn<Zombie2>(prefab, Zombie2Root, pooling);
        _zombie2s.Add(zombie);
        return zombie;
    }

    private T Spawn<T>(string prefab, Transform root, bool pooling) where T : ObjectBase
    {
        GameObject go = pooling
            ? PoolManager.Instance.Pop(prefab)
            : ResourceManager.Instance.Instantiate(prefab);

        go.name = prefab;
        go.transform.SetParent(root);

        T obj = go.GetOrAddComponent<T>();
        obj.Pooling = pooling;
        _objects.Add(obj);

        return obj;
    }

    public void Despawn(ObjectBase obj)
    {
        if (obj == null)
            return;

        _objects.Remove(obj);

        if (obj is Zombie zombie)
            _zombies.Remove(zombie);
        else if (obj is Zombie1 zombie1)
            _zombie1s.Remove(zombie1);
        else if (obj is Zombie2 zombie2)
            _zombie2s.Remove(zombie2);

        if (obj.Pooling)
        {
            obj.init();
            PoolManager.Instance.Push(obj.gameObject);
        }
        else
            ResourceManager.Instance.Destroy(obj.gameObject);
    }
}