﻿using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities.Debugging;

namespace GGJ.BubbleFall
{

    public class Bubble : MonoBehaviour
    {
        //the current state of the bubble
        private enum STATE
        {
            NONE,
            IDLE,
            HOLDING_CAPTIVE,
            THROWN,
            DEPLOYED
        }
        // the attribute 
        private enum ATTRIBUTE
        {
            NONE,
            FROZEN,
            EXPLOSIVE,
            FIRE,
            STATIC

        }
        //============================================================================================================//

        [SerializeField, ReadOnly]
        private STATE currentState = STATE.NONE;

        [SerializeField, ReadOnly]
        private ATTRIBUTE currentAttribute = ATTRIBUTE.EXPLOSIVE;

        [SerializeField, ReadOnly]
        private bool holdingObject;
        private ICanBeCaptured m_heldObject;

        [FormerlySerializedAs("decelMult")]
        [SerializeField, Min(.01f)]
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

        /// <summary>
        /// for explosions
        [SerializeField, Min(0.001f), Header("Explosion Collision Data")]
        private float explosionRadius;
        [SerializeField]
        private LayerMask explodeLayerMask;
        /// </summary>
        private Rigidbody2D m_rigidbody2D;
        private Collider2D m_collider2D;
        // [SerializeField]
        // private Rigidbody2D externalRigidbody2D;
        [SerializeField]
        private Collider2D externalCollider2D;
        //Movement Speed
        private Vector2 m_velocity;

        //For Interactions with Player
        //------------------------------------------------//
        private Collider2D[] _playerColliders;
        private bool m_didMoveFarEnough;

        private float m_minDistanceFromPlayer;
        public Vector3 m_worldStartPosition;


        // How long until the player can collide with the deployed bubble
        [SerializeField] private float captiveBubbleCooldown = 0.5f;
        private float _captiveBubbleTimer;

        //Unity Functions
        //============================================================================================================//

        private void Start()
        {

            m_rigidbody2D = GetComponent<Rigidbody2D>();
            m_collider2D = GetComponent<Collider2D>();
            m_collider2D.enabled = false;

            _playerColliders = FindAnyObjectByType<PlayerMovementV2>().GetComponents<Collider2D>();
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
                case STATE.THROWN:

                    ProcessDeployed();
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
            m_worldStartPosition = (Vector2)transform.position;
        }

        private void ProcessMove()
        {
            var newPosition = (Vector2)transform.position;

            newPosition += m_velocity * Time.deltaTime;

            //Slow down the Bubble
            m_velocity -= m_velocity * (Time.deltaTime * decelerationMultiplier);

            m_velocity += Physics2D.gravity * Time.deltaTime;

            transform.position = newPosition;
        }

        //============================================================================================================//

        private void ProcessIdle()
        {
            if (!m_didMoveFarEnough && Vector2.Distance(m_worldStartPosition, transform.position) > 1f)
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


            // Look for player feet collision
            var player = overlapCircle.GetComponentInParent<PlayerMovementV2>();
            if (player)
            {
                player.OnBubbleCollision(this, radius);
                return;
            }

            switch (actor)
            {
                case ICanBeCaptured { IsCaptured: false } canBeCaptured:
                    {
                        var other = canBeCaptured.Capture(this);
                        //If the captured object returned null, we have to assume it has destroyed itself (Or intends to)
                        if (other == null)
                        {
                            Destroy(gameObject);
                            return;
                        }
                        currentAttribute = (ATTRIBUTE)overlapCircle.GetComponent<Actor>().attribute;
                        holdingObject = true;
                        m_heldObject = canBeCaptured;

                        // Put actor in bubble
                        transform.position = other.transform.position;
                        other.transform.SetParent(transform, true);
                        //transform.SetParent(other.transform, true);

                        currentState = STATE.HOLDING_CAPTIVE;

                        break;
                    }
                    // case PlayerController playerController when m_didMoveFarEnough:
                    //     {
                    //         playerController.ExternalJump();
                    //         Destroy(gameObject);
                    //         break;
                    //     }
            }


        }

        private void ProcessHoldingCaptive()
        {
            _captiveBubbleTimer = captiveBubbleCooldown;
            m_didMoveFarEnough = false;

            var overlapCircle = Physics2D.OverlapCircle(transform.position, captiveRadius, actorLayerMask.value);

            Draw.Circle(transform.position, Color.magenta, captiveRadius);

            if (overlapCircle == null)
                return;

            var interactWithCaptive = overlapCircle.GetComponent<IInteractWithCaptive>();

            if (interactWithCaptive == null)
                return;

            interactWithCaptive.CarryCaptive(m_heldObject);

            // Give our bubble a lifetime
            m_lifeTimer = lifeTime;

            //Once picked up, we want to prepare to be thrown
            currentState = STATE.THROWN;

            // Disable button (hide from view)
            transform.gameObject.SetActive(false);

        }

        private void ProcessDeployed()
        {

            if (_captiveBubbleTimer >= 0)
            {
                captiveBubbleCooldown -= Time.deltaTime;
            }

            // m_heldObject.transform.gameObject.GetComponentInCh<BoxCollider2D>().enabled = true;
            var overlapCircle = Physics2D.OverlapCircle(transform.position, radius, actorLayerMask.value);
            externalCollider2D.gameObject.SetActive(true);
            if (!m_didMoveFarEnough && Vector2.Distance(m_worldStartPosition, transform.position) > 2f)
            {
                m_didMoveFarEnough = true;
            }

            //Countdown time until Bubble dies
            //------------------------------------------------//
            if (m_lifeTimer <= 0f)
            {
                Destroy(gameObject);
                return;
            }

            m_lifeTimer -= Time.deltaTime;

            // transform.Translate(new Vector3(0.0f, -0.1f, 0.0f));
            switch (currentAttribute)
            {
                case ATTRIBUTE.NONE:

                    if (_captiveBubbleTimer > 0)
                    {
                        //IsCaptured = true;
                        m_rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                        //m_rigidbody2D.linearVelocity = Vector3.zero;
                        m_rigidbody2D.gravityScale = 1f;
                        // Standard movement
                        //ProcessMove();
                        m_velocity = m_rigidbody2D.linearVelocity;
                        m_velocity -= m_velocity * (Time.deltaTime * decelerationMultiplier);
                        m_velocity += Physics2D.gravity * Time.deltaTime;

                        m_rigidbody2D.linearVelocity = m_velocity;
                    }
                    var overlapCircle2 = Physics2D.OverlapCircle(transform.position, radius, actorLayerMask.value);
                    if (overlapCircle2 == null)
                        return;
                    Debug.Log("I hit a thing");
                    var exploded = Physics2D.OverlapCircle(transform.position, explosionRadius, explodeLayerMask.value);
                    if (exploded == null)
                    {
                        return;
                    }
                    Draw.Circle(transform.position, Color.red, explosionRadius);
                    Debug.Log(exploded);

                    Destroy(exploded.gameObject);
                    Destroy(gameObject);
                    break;
                case ATTRIBUTE.FIRE:
                    //m_rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                    m_rigidbody2D.linearVelocity = Vector3.zero;
                    m_rigidbody2D.freezeRotation = true;
                    m_rigidbody2D.gravityScale = -0.01f;
                    m_rigidbody2D.linearDamping = 0.1f;
                    break;
                    //m_heldObject.transform.position += new Vector3(0.0f, 0.1f, 0.0f);u
                    // TODO -- do fire placement here
            }
        }


        public IEnumerator IgnoreCollisionCoroutine(float duration)
        {
            foreach (var col in _playerColliders)
            {
                Physics2D.IgnoreCollision(externalCollider2D, col, true);
            }
            yield return new WaitForSeconds(duration);
            foreach (var col in _playerColliders)
            {
                Physics2D.IgnoreCollision(externalCollider2D, col, false);
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