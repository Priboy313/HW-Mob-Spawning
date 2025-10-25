using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnHandler : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] List<Spawner> _spawnerPool = new();
    [SerializeField] Mob _prefabMob;
    [SerializeField] float _spawnDelay = 2f;

    [Header("Spawned Mobs Lifetime")]
    [SerializeField, Min(2)] private float _lifetimeMin = 20;
    [SerializeField, Min(5)] private float _lifetimeMax = 50;

    [Header("Mobs Pool")]
    [SerializeField] private int _mobPoolCapacity = 20;
    [SerializeField] private int _mobPoolMaxSize = 100;

    private ObjectPool<Mob> _mobPool;

    private void OnValidate()
    {
        if (_lifetimeMax <= _lifetimeMin)
        {
            _lifetimeMax = _lifetimeMin + 1;
        }

        if (_mobPoolMaxSize < _mobPoolCapacity)
        {
            _mobPoolMaxSize = _mobPoolCapacity;
        }
    }

    private void Awake()
    {
        _mobPool = new ObjectPool<Mob>(
            createFunc: CreatePooledObject,
            actionOnGet: (mob) => OnTakeFromPool(mob),
            actionOnRelease: (mob) => mob.gameObject.SetActive(false),
            actionOnDestroy: OnDestroyFromPool,
            collectionCheck: true,
            defaultCapacity: _mobPoolCapacity,
            maxSize: _mobPoolCapacity
        );
    }

    private void Start()
    {
        if (_spawnerPool.Count > 0)
        {
            StartCoroutine(SpawnObjectOfPool());
        }
        else
        {
            Debug.LogError("Не установлены точки спавна!");
        }
    }

    private Mob CreatePooledObject()
    {
        Mob mob = Instantiate(_prefabMob);
        mob.ActionLifetimeOut += OnLifetimeOut;

        return mob;
    }

    private void OnTakeFromPool(Mob mob)
    {
        Spawner spawner = _spawnerPool[DevUtils.GetRandomNumber(_spawnerPool.Count)];

        mob.transform.position = spawner.SpawnPosition;
        mob.gameObject.SetActive(true);
        ResetMob(mob, spawner.MoveDirection);
    }

    private void ResetMob(Mob mob, Vector3 direction)
    {
        mob.Init(direction, _lifetimeMin, _lifetimeMax);
        mob.Rigidbody.velocity = Vector3.zero;
        mob.Rigidbody.angularVelocity = Vector3.zero;
        mob.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnDestroyFromPool(Mob mob)
    {
        mob.ActionLifetimeOut -= OnLifetimeOut;
        Destroy(mob.gameObject);
    }

    private IEnumerator SpawnObjectOfPool()
    {
        var wait = new WaitForSeconds(_spawnDelay);

        while (enabled)
        {
            _mobPool.Get();

            yield return wait;
        }
    }

    private void OnLifetimeOut(Mob mob)
    {
        _mobPool.Release(mob);
    }

}
