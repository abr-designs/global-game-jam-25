﻿using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities.Debugging;

namespace Protoype.Alex_Side_Scroller
{
    
    public class Bubble : MonoBehaviour
    {
        private enum STATE
        {
            NONE,
            IDLE,
            HOLDING_CAPTIVE
        }
        
        //============================================================================================================//

        [SerializeField, ReadOnly]
        private STATE currentState = STATE.NONE;
        
        [SerializeField, ReadOnly]
        private bool holdingObject;
        private ICanBeCaptured m_heldObject;
        
        [FormerlySerializedAs("decelMult")] [SerializeField, Min(1f)]
        private float decelerationMultiplier = 1f;

        [SerializeField, Min(0.001f)]
        private float lifeTime = 1f;
        private float m_lifeTimer;

        [SerializeField, Min(0.001f), Header("Idle Collision Data")]
        private float radius;
        [SerializeField]
        private LayerMask actorLayerMask;
        
        [SerializeField, Min(0.001f), Header("Holding Captive Collision Data")]
        private float captiveRadius;

        private Rigidbody2D m_rigidbody2D;
        private Collider2D m_collider2D;
        
        //Movement Speed
        private Vector2 m_velocity;

        //For Interactions with Player
        //------------------------------------------------//
        
        private bool m_didMoveFarEnough;
        
        private float m_minDistanceFromPlayer;
        private Vector3 m_worldStartPosition;

        //Unity Functions
        //============================================================================================================//

        private void Start()
        {
            m_worldStartPosition = transform.position;
            
            m_rigidbody2D = GetComponent<Rigidbody2D>();
            m_collider2D = GetComponent<Collider2D>();
            m_collider2D.enabled = false;
        }

        private void Update()
        {
            switch (currentState)
            {
                case STATE.IDLE:
                    ProcessIdle();
                    break;
                case STATE.HOLDING_CAPTIVE:
                    ProcessHoldingCaptive();
                    break;
                case STATE.NONE:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //============================================================================================================//

        public void Init(Vector2 velocity)
        {
            m_velocity = velocity;
            m_lifeTimer = lifeTime;

            currentState = STATE.IDLE;
        }

        private void ProcessMove()
        {
            var newPosition = (Vector2)transform.position;
            
            newPosition += m_velocity * Time.deltaTime;

            //Slow down the Bubble
            m_velocity -= m_velocity * (Time.deltaTime * decelerationMultiplier);

            transform.position = newPosition;
        }
        
        //============================================================================================================//

        private void ProcessIdle()
        {
            if (!m_didMoveFarEnough && (m_worldStartPosition - transform.position).magnitude > 1f)
                m_didMoveFarEnough = true;

            //Countdown time until Bubble dies
            //------------------------------------------------//
            if (m_lifeTimer <= 0f)
            {
                Destroy(gameObject);
                return;
            }

            m_lifeTimer -= Time.deltaTime;
            
            //------------------------------------------------//
            
            ProcessMove();

            var overlapCircle = Physics2D.OverlapCircle(transform.position, radius, actorLayerMask.value);
            
            Draw.Circle(transform.position, Color.magenta, radius);

            if (overlapCircle == null)
                return;
            
            var actor = overlapCircle.GetComponent<ICanBeBubbled>();

            switch (actor)
            {
                case ICanBeCaptured canBeCaptured:
                {
                    
                    var other = canBeCaptured.Capture();
                    //If the captured object returned null, we have to assume it has destroyed itself (Or intends to)
                    if (other == null)
                    {
                        Destroy(gameObject);
                        return;
                    }
                    
                    holdingObject = true;
                    m_heldObject = canBeCaptured;

                    transform.position = other.transform.position;
                    transform.SetParent(other.transform, true);

                    currentState = STATE.HOLDING_CAPTIVE;
                    break;
                }
                case PlayerController playerController when m_didMoveFarEnough:
                {
                    playerController.ExternalJump();
                    Destroy(gameObject);
                    break;
                }
            }

        }

        private void ProcessHoldingCaptive()
        {
            var overlapCircle = Physics2D.OverlapCircle(transform.position, captiveRadius, actorLayerMask.value);
            
            Draw.Circle(transform.position, Color.magenta, captiveRadius);

            if (overlapCircle == null)
                return;
            
            var actor = overlapCircle.GetComponent<ICanBeBubbled>();

            switch (actor)
            {
                case ICanBeCaptured canBeCaptured:
                {
                    //TODO Maybe other things can pop the bubble?
                    break;
                }
                case PlayerController playerController:
                {
                    //TODO When the player gets close enough to hold this object do so here
                    break;
                }
            }
        }

        //Title
        //============================================================================================================//
        
        #if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Draw.Label(transform.position, $"State: {currentState.ToString()}");
        }

#endif
    }
}