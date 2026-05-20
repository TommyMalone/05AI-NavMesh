using System;
using UnityEngine;
using UnityEngine.AI;

public class FollowWaypoint : MonoBehaviour
{
    public GameObject waypointManager;
    private GameObject[] _waypoints;
    private GameObject _currentNode;
    private NavMeshAgent _navMeshAgent;

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _waypoints = waypointManager.GetComponent<WaypointManager>().waypoints;
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void GoToWaypoint(int index)
    {
        _navMeshAgent.SetDestination(_waypoints[index].transform.position);
    }

    private void LateUpdate()
    {
       
    }
}
