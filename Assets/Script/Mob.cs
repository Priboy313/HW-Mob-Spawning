using System;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private Rigidbody _rigidbody;
    private TargetPoint _currentTarget;

    public Rigidbody Rigidbody => _rigidbody;

    public event Action<Mob> ActionReadyForRelease;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Init(Vector3 spawnPosition, TargetPoint targetPoint)
    {
        transform.position = spawnPosition;
        _currentTarget = targetPoint;
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
        _rigidbody.velocity = new Vector3(targetVelocity.x, _rigidbody.velocity.y, targetVelocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<TargetPoint>(out TargetPoint collidedTarget))
        {
            if (collidedTarget == _currentTarget)
            {
                ActionReadyForRelease?.Invoke(this);
            }
        }
    }
}
