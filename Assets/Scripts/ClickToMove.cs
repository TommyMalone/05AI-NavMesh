using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Holistic3D.Navigation {

    public class ClickToMove : MonoBehaviour {

        [SerializeField] Camera _camera;
        [SerializeField] NavMeshAgent _agent;
        [SerializeField] LayerMask _clickableMask = ~0; // which layers are valid to click (must have colliders)
        [SerializeField] float _simpleMaskDistance = 2.0f;  // how far we'll "snap" to the navmesh
        [SerializeField] int _areaMask = NavMesh.AllAreas;  // or bitmask of specific areas
        [SerializeField] float _raycastDistance = 1000.0f;

        private void Reset() {
            
            _camera = Camera.main;
            _agent = GetComponent<NavMeshAgent>();
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
            if (_camera != null && _agent != null)
            {
                Ray ray = _camera.ScreenPointToRay(screenPosition);
                if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance, _clickableMask, QueryTriggerInteraction.Ignore))
                {
                    if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, _simpleMaskDistance, _areaMask))
                    {
                        _agent.SetDestination(navHit.position);
                    }
                }
            }
        }

        // Optional: visualise the last destination in Scene view
        private void OnDrawGizmosSelected() {
            
            if(_agent != null) {

                Gizmos.DrawWireSphere(_agent.destination, 0.2f);
            }
        }
    }
}