using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MobPoolsHandler : MonoBehaviour
{
    [Header("Mobs Pool")]
    [SerializeField] private int _mobPoolCapacity = 20;
    [SerializeField] private int _mobPoolMaxSize = 100;

    private Dictionary<Mob, ObjectPool<Mob>> _mobPools = new();
    private Dictionary<Mob, Mob> _instanceToPrefabMap = new();

    private void OnValidate()
    {
        if (_mobPoolMaxSize < _mobPoolCapacity)
        {
            _mobPoolMaxSize = _mobPoolCapacity;
        }
    }

    public Mob GetMob(Mob prefab)
    {
        if (_mobPools.ContainsKey(prefab) == false)
        {
            CreatePool(prefab);
        }

        Mob mobInstance = _mobPools[prefab].Get();
        _instanceToPrefabMap[mobInstance] = prefab;

        return mobInstance;
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
        mob.ActionReadyForRelease += OnMobReadyForRelease;
        return mob;
    }

    private void OnMobReadyForRelease(Mob mobInstance)
    {
        if (_instanceToPrefabMap.TryGetValue(mobInstance, out Mob prefab))
        {
            _mobPools[prefab].Release(mobInstance);
        }
    }

    private void OnDestroyFromPool(Mob mobInstance)
    {
        mobInstance.ActionReadyForRelease -= OnMobReadyForRelease;
        _instanceToPrefabMap.Remove(mobInstance);
        Destroy(mobInstance.gameObject);
    }
}
