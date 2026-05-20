using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Holistic3D.Navigation {

    public class ClickToMove : MonoBehaviour {

        [SerializeField] Camera _camera;
        [SerializeField] NavMeshAgent _agent;
        [SerializeField] LayerMask _clickableMask = ~0;
        [SerializeField] float _simpleMaskDistance = 2.0f;
        [SerializeField] int _areaMask = NavMesh.AllAreas;

        private void Reset() {
            
            _camera = Camera.main;
            _agent = GetComponent<NavMeshAgent>();
        }

        void Update() {

            if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;

            // Mouse Click
            //if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
               // TrySetDestination(Mouse.current.position.ReadValue());

            //if(Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
                //TrySetDestination(Touchscreen.current.primaryTouch.position.ReadValue());
        }

       



        // Optional: visualise the last destination in Scene view
        private void OnDrawGizmosSelected() {
            
            if(_agent != null) {

                Gizmos.DrawWireSphere(_agent.destination, 0.2f);
            }
        }
    }
}