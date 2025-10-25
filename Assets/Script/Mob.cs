using System;
using System.Collections;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [SerializeField] private float _speed = 2;
    [SerializeField] private Vector3 _direction;

    private float _lifetimeMin = 50;
    private float _lifetimeMax = 100;

    private Rigidbody _rigidbody;

    public Rigidbody Rigidbody => _rigidbody;

    public event Action<Mob> ActionLifetimeOut;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Init(Vector3 direction, float lifetimeMIn, float lifetimeMax)
    {
        _direction = direction.normalized;

        _lifetimeMin = lifetimeMIn;
        _lifetimeMax = lifetimeMax;

        StartCoroutine(StartTimerToDestroy());
    }

    private void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;

        Quaternion targetRotation = Quaternion.LookRotation(_direction);
        transform.rotation = targetRotation;
    }

    private IEnumerator StartTimerToDestroy()
    {
        yield return new WaitForSeconds(DevUtils.GetRandomNumber(_lifetimeMin, _lifetimeMax + 1f));

        ActionLifetimeOut?.Invoke(this);
    }
}
