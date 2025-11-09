using System;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private Target _currentTarget;

    public Rigidbody Rigidbody { get; private set; }

    public event Action<Mob> ActionReadyForRelease;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void Init(SpawnPoint spawnPoint)
    {
        transform.position = spawnPoint.SpawnPosition;
        _currentTarget = spawnPoint.Target;
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        if (_currentTarget == null)
        {
            Debug.LogError("Current Target is null!");
            return;
        }

        Vector3 direction = (_currentTarget.Position - transform.position).normalized;

        Vector3 targetVelocity = direction * _speed;
        Rigidbody.velocity = new Vector3(targetVelocity.x, Rigidbody.velocity.y, targetVelocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Target>(out Target collidedTarget))
        {
            if (collidedTarget == _currentTarget)
            {
                ActionReadyForRelease?.Invoke(this);
            }
        }
    }
}
