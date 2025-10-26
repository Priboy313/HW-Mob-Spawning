using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    [SerializeField] private List<Waypoint> _waypoints = new();
    [SerializeField] private float _speed = 2;

    private int _currentWaypointIndex = 0;
    private float _reachDistance = 0.1f;

    public Vector3 Position => transform.position;
    private Vector3 CurrentTargetPosition => _waypoints[_currentWaypointIndex].Position;

    private void Start()
    {
        if (_waypoints.Count <= 0)
        {
            Debug.LogError("Waypoints is not set!");
            this.enabled = false;
        }
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(Position, CurrentTargetPosition, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, CurrentTargetPosition) <_reachDistance)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Count;
        }
    }
}
