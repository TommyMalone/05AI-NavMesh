using System;
using UnityEngine;

public class FollowWaypoint : MonoBehaviour
{
    private Transform _target;
    private float _speed = 10f;
    private float _accuracy = 3f;
    private float _rotationalSpeed = 5f;

    public GameObject waypointManager;
    private GameObject[] _waypoints;
    private GameObject _currentNode;
    private int _currentWaypointInPath = 0;
    private Graph _navigationGraph;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _waypoints = waypointManager.GetComponent<WaypointManager>().waypoints;
        _navigationGraph = waypointManager.GetComponent<WaypointManager>().graph;
        _currentNode = _waypoints[_currentWaypointInPath];
    }

    public void GoToWaypoint(int index)
    {
        //We are starting on a new path
        _currentWaypointInPath = 0;

        _navigationGraph.AStar(_currentNode, _waypoints[index]);
        
    }

    private void LateUpdate()
    {
        if (_navigationGraph.GetPathList().Count != 0 && _currentWaypointInPath != _navigationGraph.GetPathList().Count)
        {
            if (Vector3.Distance(_navigationGraph.GetPathList()[_currentWaypointInPath].GetId().transform.position,
                    transform.position) < _accuracy)
            {
                _currentNode = _navigationGraph.GetPathList()[_currentWaypointInPath].GetId();
                _currentWaypointInPath++;
            }
            if(_currentWaypointInPath < _navigationGraph.GetPathList().Count)
            {
                _target = _navigationGraph.GetPathList()[_currentWaypointInPath].GetId().transform;
                Vector3 lookAtGoal = new Vector3(_target.position.x, transform.position.y, _target.position.z);
                Vector3 direction = lookAtGoal - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * _rotationalSpeed);
                transform.Translate(Vector3.forward * _speed * Time.deltaTime);
            }
        }
    }
}
