using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<SpawnPoint> _spawnPoints = new();
    [SerializeField] private Mob _prefabMob;

    private MobPoolsHandler _mobPoolsHandler;
    
    private void Start()
    {
        if (_prefabMob == null)
        {
            Debug.LogError("Prefab is not set!");
            this.enabled = false;
        }
    }

    public void Init(MobPoolsHandler mobPoolsHandler)
    {
        _mobPoolsHandler = mobPoolsHandler;
    }

    public void SpawnMob()
    {
        if (_mobPoolsHandler == null)
        {
            Debug.LogError("MobPoolsHandler not set!");
            return;
        }

        SpawnPoint spawnPoint = _spawnPoints[DevUtils.GetRandomNumber(_spawnPoints.Count)];
        Mob mob = _mobPoolsHandler.GetMob(_prefabMob);
        ResetMob(mob, spawnPoint);
    }

    private void ResetMob(Mob mob, SpawnPoint spawnPoint)
    {
        mob.Init(spawnPoint.SpawnPosition, spawnPoint.TargetPoint);
        mob.Rigidbody.velocity = Vector3.zero;
        mob.Rigidbody.angularVelocity = Vector3.zero;
        mob.transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
