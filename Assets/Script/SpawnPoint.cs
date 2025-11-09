using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private Target _target;

    public Vector3 SpawnPosition => _spawnPosition.position;
    public Target Target => _target;

    private void Start()
    {
        if (_target == null)
        {
            Debug.LogError("Target is not set!");
            return;
        }
    }
}
