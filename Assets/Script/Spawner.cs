using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] private List<SpawnPoint> _spawnPoints = new();
    [SerializeField] private Mob _prefabMob;

    [Header("Pool")]
    [SerializeField] private int _poolCapacity = 20;
    [SerializeField] private int _poolMaxSize = 100;

    private ObjectPool<Mob> _mobPool;

    private void OnValidate()
    {
        if (_poolMaxSize < _poolCapacity)
        {
            _poolMaxSize = _poolCapacity;
        }
    }

    private void Awake()
    {
        if (_prefabMob == null)
        {
            Debug.LogError("Prefab is not set!");
            enabled = false;
        }

        _mobPool = new ObjectPool<Mob>(
            createFunc: () => CreatePooledObject(_prefabMob),
            actionOnGet: (mob) => mob.gameObject.SetActive(true),
            actionOnRelease: (mob) => mob.gameObject.SetActive(false),
            actionOnDestroy: (mob) => OnDestroyFromPool(mob),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
        );
    }

    public void SpawnMob()
    {
        SpawnPoint spawnPoint = _spawnPoints[DevUtils.GetRandomNumber(_spawnPoints.Count)];
        Mob mob = _mobPool.Get();
        mob.Init(spawnPoint);
    }

    private Mob CreatePooledObject(Mob prefab)
    {
        Mob mob = Instantiate(prefab);
        mob.ActionReadyForRelease += OnReadyForRelease;
        return mob;
    }

    private void OnReadyForRelease(Mob mob)
    {
        _mobPool.Release(mob);
    }

    private void OnDestroyFromPool(Mob mobInstance)
    {
        mobInstance.ActionReadyForRelease -= OnReadyForRelease;
        Destroy(mobInstance.gameObject);
    }
}
