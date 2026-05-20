using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Holistic3D.Navigation {

    [Flags]
    public enum NavAreaMask
    {
        Walkable = 1 << 0,
        NotWalkable = 1 << 1,
        Jump = 1 << 2,
        Water = 1 << 3,
        Concrete = 1 <<4,
    }
    public class ClickToMove : MonoBehaviour {

        [SerializeField] private Camera myCamera;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private LayerMask clickableMask = ~0; // which layers are valid to click (must have colliders)
        [SerializeField] private float simpleMaskDistance = 2.0f;  // how far we'll "snap" to the navmesh
        [SerializeField] private NavAreaMask allowedAreasMask = NavAreaMask.Walkable | NavAreaMask.Water;
        [SerializeField] private float raycastDistance = 1000.0f;
        private void Reset() {
            
            myCamera = Camera.main;
            agent = GetComponent<NavMeshAgent>();
        }

        void Update() {

            if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;

            // Mouse Click
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                TrySetDestination(Mouse.current.position.ReadValue());

            if(Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
                TrySetDestination(Touchscreen.current.primaryTouch.position.ReadValue());
        }


        void TrySetDestination(Vector2 screenPosition)
        {
            if (myCamera != null && agent != null)
            {
                Ray ray = myCamera.ScreenPointToRay(screenPosition);
                bool didHit = Physics.Raycast(ray, out RaycastHit hit, raycastDistance, clickableMask,
                    QueryTriggerInteraction.Ignore);
                if (didHit)
                {
                    if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, simpleMaskDistance, (int)allowedAreasMask))
                    {
                        agent.SetDestination(navHit.position);
                    }
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
}