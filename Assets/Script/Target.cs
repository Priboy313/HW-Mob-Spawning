using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private List<Waypoint> _waypoints = new();
    [SerializeField] private float _speed = 2;

    private int _currentWaypointIndex = 0;
    private float _reachDistance = 0.1f;
    private float _reachDistanceSqr;

    public Vector3 Position => transform.position;

    private void Start()
    {
        if (_waypoints.Count <= 0)
        {
            Debug.LogError("Waypoints is not set!");
            enabled = false;
        }

        _reachDistanceSqr = _reachDistance * _reachDistance;
    }

    private void Update()
    {
        Vector3 currentTargetPosition = _waypoints[_currentWaypointIndex].Position;

        transform.position = Vector3.MoveTowards(Position, currentTargetPosition, _speed * Time.deltaTime);

        float sqrDistance = (currentTargetPosition - transform.position).sqrMagnitude;

        if (sqrDistance < _reachDistanceSqr)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Count;
        }
    }
}
