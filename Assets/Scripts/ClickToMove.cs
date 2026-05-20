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

        [SerializeField] private Camera _camera;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private LayerMask _clickableMask = ~0; // which layers are valid to click (must have colliders)
        [SerializeField] private float _simpleMaskDistance = 2.0f;  // how far we'll "snap" to the navmesh
        [FormerlySerializedAs("_allowedAreas")] [SerializeField] private NavAreaMask _allowedAreasMask = NavAreaMask.Walkable | NavAreaMask.Water;
        [SerializeField] private float _raycastDistance = 1000.0f;
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
                    if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, _simpleMaskDistance, (int)_allowedAreasMask))
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