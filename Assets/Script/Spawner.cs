using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Vector3 _moveDirection = Vector3.forward;

    public Vector3 SpawnPosition => _spawnPoint.position;
    public Vector3 MoveDirection => _moveDirection;

    private void Awake()
    {
        _moveDirection = DevUtils.GetRandomDirection2D();
    }
}
