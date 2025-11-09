using System.Collections;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private TargetPoint _targetPoint;

    public Vector3 SpawnPosition => _spawnPosition.position;
    public TargetPoint TargetPoint => _targetPoint;

    private void Start()
    {
        if (_targetPoint == null)
        {
            Debug.LogError("Target is not set!");
            return;
        }
    }
}
