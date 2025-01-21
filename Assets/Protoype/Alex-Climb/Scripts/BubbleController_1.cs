using System;
using UnityEngine;

namespace Protoype.Alex_Climb
{
    public class BubbleController_1 : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D bubbleRigidBody;

        [SerializeField, Min(0f)]
        private float pushForce;

        private Camera mainCamera;
        private Vector3 _startingPosition;

        //Unity Functions
        //============================================================================================================//
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            mainCamera = Camera.main;
            _startingPosition = bubbleRigidBody.position;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                Reset();
            
            if (Input.GetKeyDown(KeyCode.Mouse0) == false)
                return;

            var clickPosition = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);

            var dir = (bubbleRigidBody.position - clickPosition).normalized;
            
            bubbleRigidBody.AddForceAtPosition(dir * pushForce, clickPosition);
        }

        private void Reset()
        {
            bubbleRigidBody.position = _startingPosition;
            bubbleRigidBody.linearVelocity = Vector2.zero;
        }

        //============================================================================================================//
    }
}
