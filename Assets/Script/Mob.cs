using System;
using System.Collections;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private float _lifetimeMin = 50;
    private float _lifetimeMax = 100;

    private Vector3 _direction;
    private Rigidbody _rigidbody;

    public Rigidbody Rigidbody => _rigidbody;

    public event Action<Mob> ActionLifetimeOut;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Init(Vector3 direction, float lifetimeMIn, float lifetimeMax)
    {
        _direction = direction;

        _lifetimeMin = lifetimeMIn;
        _lifetimeMax = lifetimeMax;

        StartCoroutine(StartTimerToDestroy());
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = _direction * _speed;
        _rigidbody.velocity = new Vector3(targetVelocity.x, _rigidbody.velocity.y, targetVelocity.z);
    }

    private IEnumerator StartTimerToDestroy()
    {
        yield return new WaitForSeconds(DevUtils.GetRandomNumber(_lifetimeMin, _lifetimeMax + 1f));

        ActionLifetimeOut?.Invoke(this);
    }
}
