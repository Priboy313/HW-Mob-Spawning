using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MobPoolsHandler : MonoBehaviour
{
    [Header("Mobs Pool")]
    [SerializeField] private int _mobPoolCapacity = 20;
    [SerializeField] private int _mobPoolMaxSize = 100;

    public static MobPoolsHandler Instance;

    private Dictionary<Mob, ObjectPool<Mob>> _mobPools = new();

    private void OnValidate()
    {
        if (_mobPoolMaxSize < _mobPoolCapacity)
        {
            _mobPoolMaxSize = _mobPoolCapacity;
        }
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public Mob GetMob(Mob prefab)
    {
        if (_mobPools.ContainsKey(prefab) == false)
        {
            CreatePool(prefab);
        }

        return _mobPools[prefab].Get();
    }

    private void CreatePool(Mob prefab)
    {
        ObjectPool<Mob> pool = new ObjectPool<Mob>(
            createFunc: () => CreatePooledObject(prefab),
            actionOnGet: (mob) => mob.gameObject.SetActive(true),
            actionOnRelease: (mob) => mob.gameObject.SetActive(false),
            actionOnDestroy: (mob) => OnDestroyFromPool(mob),
            collectionCheck: true,
            defaultCapacity: _mobPoolCapacity,
            maxSize: _mobPoolCapacity
        );

        _mobPools.Add(prefab, pool);
    }

    private Mob CreatePooledObject(Mob prefab)
    {
        Mob mob = Instantiate(prefab);
        mob.ActionTargetPointReached += OnMobReachedTarget;
        return mob;
    }

    private void OnMobReachedTarget(Mob mob)
    {
        _mobPools[mob.Prefab].Release(mob);
    }

    private void OnDestroyFromPool(Mob mob)
    {
        mob.ActionTargetPointReached -= OnMobReachedTarget;
        Destroy(mob.gameObject);
    }
}
