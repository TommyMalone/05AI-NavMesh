using Holistic3D.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class FollowTarget : MonoBehaviour
{
    [FormerlySerializedAs("player")] [SerializeField] private GameObject target;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float simpleMaskDistance = 2.0f;  // how far we'll "snap" to the navmesh
    [SerializeField] private NavAreaMask allowedAreasMask = NavAreaMask.Walkable | NavAreaMask.Water;
    [SerializeField] private float raycastDistance = 1000.0f;
    private void Reset()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target != null)
        {
            TrySetDestination(target.transform.position);
        }
    }


    void TrySetDestination(Vector3 destination)
    {
        if (agent != null)
        {
            if (NavMesh.SamplePosition(destination, out NavMeshHit navHit, simpleMaskDistance, (int)allowedAreasMask))
            {
                agent.SetDestination(navHit.position);
            }
            
        }
    }

    // Optional: visualise the last destination in Scene view
    private void OnDrawGizmosSelected() {
        
        if(agent != null) {

            Gizmos.DrawWireSphere(agent.destination, 0.2f);
        }
    }
}

