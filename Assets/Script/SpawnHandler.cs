using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MobPoolsHandler))]
public class SpawnHandler : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] private List<Spawner> _spawners = new();
    [SerializeField, Min(0.1f)] private float _spawnDelay = 2f;

    private MobPoolsHandler _mobPoolsHandler;

    private void Awake()
    {
        _mobPoolsHandler = GetComponent<MobPoolsHandler>();

        foreach (Spawner spawner in _spawners)
        {
            spawner.Init(_mobPoolsHandler);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnObjectOfPool());
    }

    private IEnumerator SpawnObjectOfPool()
    {
        var wait = new WaitForSeconds(_spawnDelay);

        while (enabled)
        {
            Spawner spawner = _spawners[DevUtils.GetRandomNumber(_spawners.Count)];
            spawner.SpawnMob();
            yield return wait;
        }
    }

}
