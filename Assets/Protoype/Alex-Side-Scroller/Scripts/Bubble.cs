using System;
using NaughtyAttributes;
using UnityEngine;
using Utilities.Debugging;

namespace Protoype.Alex_Side_Scroller
{
    
    public class Bubble : MonoBehaviour
    {
        private enum STATE
        {
            NONE,
            IDLE,
            CAPTURED
        }
        
        //============================================================================================================//
        [SerializeField, ReadOnly]
        private bool holdingObject;
        [SerializeReference]
        private ICanBeCaptured heldObject;
        
        [SerializeField, Min(1f)]
        private float decelMult = 1f;

        [SerializeField, Min(0.001f)]
        private float lifeTime = 1f;
        private float m_lifeTimer;

        private float m_playerJumpDelay = 1f;

        [SerializeField, Header("Collision")]
        private float radius;
        [SerializeField]
        private LayerMask collisionMask;
        private Vector2 m_velocity;

        private Rigidbody2D m_rigidbody2D;
        private Collider2D m_collider2D;

        private STATE m_currentState;

        private Vector3 m_worldStartPosition;
        private bool didMoveFarEnough;

        private void Start()
        {
            m_worldStartPosition = transform.position;
            
            m_rigidbody2D = GetComponent<Rigidbody2D>();
            m_collider2D = GetComponent<Collider2D>();
            m_collider2D.enabled = false;
        }

        private void Update()
        {
            switch (m_currentState)
            {
                case STATE.IDLE:
                    ProcessIdle();
                    break;
                case STATE.CAPTURED:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        //============================================================================================================//


        private void ProcessIdle()
        {
            if (!didMoveFarEnough && (m_worldStartPosition - transform.position).magnitude > 1f)
                didMoveFarEnough = true;
            //m_playerJumpDelay -= Time.deltaTime;
            
            if (m_lifeTimer <= 0f)
            {
                Destroy(gameObject);
                return;
            }

            m_lifeTimer -= Time.deltaTime;
            
            ProcessMove();

            var overlapCircle = Physics2D.OverlapCircle(transform.position, radius, collisionMask.value);
            
            Draw.Circle(transform.position, Color.magenta, radius);

            if (overlapCircle == null)
                return;
            
            var @interface = overlapCircle.GetComponent<ICanInterface>();

            switch (@interface)
            {
                case ICanBeCaptured canBeCaptured:
                    holdingObject = true;
                    heldObject = canBeCaptured;
                    var other = canBeCaptured.Capture();

                    if (other == null)
                    {
                        Destroy(gameObject);
                        return;
                    }

                    transform.position = other.transform.position;
                    transform.SetParent(other.transform, true);
                    //TODO Just do this with the physics components
                    //transform.position = other.transform.position;
                    //other.transform.SetParent(transform, true);
                    //m_rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    //m_collider2D.enabled = true;
                    
                    m_currentState = STATE.CAPTURED;
                    break;
                //case PlayerController playerController when didMoveFarEnough:
                //    playerController.ExternalJump();
                //    Destroy(gameObject);
                //    break;
            }

        }
        

        //============================================================================================================//

        public void Init(Vector2 velocity)
        {
            m_velocity = velocity;
            m_lifeTimer = lifeTime;

            m_currentState = STATE.IDLE;
        }

        private void ProcessMove()
        {
            var newPosition = (Vector2)transform.position;
            
            newPosition += m_velocity * Time.deltaTime;

            //Slow down the Bubble
            m_velocity -= m_velocity * (Time.deltaTime * decelMult);

            transform.position = newPosition;
        }
    }
}