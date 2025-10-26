using System.Collections;
using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float _spawnDelay = 2f;
    [SerializeField] private Mob _prefabMob;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private TargetPoint _targetPoint;

    private void Start()
    {
        if (_prefabMob == null || _targetPoint == null)
        {
            Debug.LogError("Prefab or Target not set!");
            return;
        }

        StartCoroutine(SpawnObjectOfPool());
    }

    private void ResetMob(Mob mob)
    {
        mob.Init(_spawnPoint.position, _targetPoint, _prefabMob);
        mob.Rigidbody.velocity = Vector3.zero;
        mob.Rigidbody.angularVelocity = Vector3.zero;
        mob.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private IEnumerator SpawnObjectOfPool()
    {
        var wait = new WaitForSeconds(_spawnDelay);

        while (enabled)
        {
            ResetMob(MobPoolsHandler.Instance.GetMob(_prefabMob));
            yield return wait;
        }
    }
}
