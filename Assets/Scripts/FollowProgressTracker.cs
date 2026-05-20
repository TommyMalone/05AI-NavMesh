using UnityEngine;

public class FollowProgressTracker : MonoBehaviour
{
    public GameObject[] waypoints;
    int _currentWaypointIndex = 0;
    public float speed = 10f;
    public float rotationalSpeed = 5f;
    public int waypointArrivalThreshold = 5;
    private GameObject _tracker;
    private int _trackerSpeedBoost = 2;
    public float lookAhead = 10f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tracker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        DestroyImmediate(_tracker.GetComponent<Collider>());
        _tracker.transform.position = transform.position;
        _tracker.transform.rotation = transform.rotation;
    }

    void ProgressTracker()
    {
        if (Vector3.Distance(_tracker.transform.position, transform.position) < lookAhead)
        {
            if (Vector3.Distance(_tracker.transform.position, waypoints[_currentWaypointIndex].transform.position) <
                waypointArrivalThreshold)
            {
                _currentWaypointIndex++;
            }

            if (_currentWaypointIndex >= waypoints.Length)
            {
                _currentWaypointIndex = 0;
            }

            _tracker.transform.LookAt(waypoints[_currentWaypointIndex].transform);
            _tracker.transform.Translate(_tracker.transform.forward * (speed + _trackerSpeedBoost) * Time.deltaTime,
                Space.World);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProgressTracker();
        
        Quaternion lookAtWaypoint = Quaternion.LookRotation(_tracker.transform.position - transform.position);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAtWaypoint, Time.deltaTime * rotationalSpeed);
        
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
