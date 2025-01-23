using System;
using NaughtyAttributes;
using UnityEngine;

namespace Protoype.Alex_Side_Scroller
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour, ICanInterface
    {
        public static event Action<Vector2> DidJump;
    
        [Header("Movement")]
        //------------------------------------------------//
        [SerializeField, Min(0f)]private float moveSpeed;

        [SerializeField, Min(0f)]private float moveAcceleration;
        [SerializeField, Min(0f)]private float moveDeceleration;

        private float m_currentMoveForce;
        private float m_moveSmoothDampVelocity;

        [Header("Jump")]
        //------------------------------------------------//
        [SerializeField, Min(0f)]private float jumpForce;
        [SerializeField, Min(0f)]private float jumpMaxHoldTime;
        private float m_jumpHoldTimer;
        [SerializeField, Min(1f)]private float downForceMult = 2f;
        
   
        [Header("Inputs")]
        //------------------------------------------------//
        [SerializeField]private KeyCode moveLeftKeycode = KeyCode.A;
        [SerializeField]private KeyCode moveRightKeycode = KeyCode.D;
        [SerializeField]private KeyCode jumpKeycode = KeyCode.Space;
        private bool isPressingJump;

        //TODO Determine if I even need W/S
        [SerializeField, ReadOnly]private int xInput;

        [SerializeField]
        private LayerMask levelMask;
   
        [SerializeField, Header("Ground Checks")]
        //------------------------------------------------//
        private float castStartYPosition;
        [SerializeField, ReadOnly]
        private bool isGrounded;
        private static readonly float[] CastXPositions = new []
        {
            -0.1f,
            0.0f,
            0.1f
        };
   
        [Header("Wall Checks")]
        //------------------------------------------------//
        [SerializeField]
        private float castStartXPosition;
        [SerializeField, ReadOnly]
        private bool hittingWall;
        private static readonly float[] CastYPositions = new []
        {
            -0.33f,
            0.0f,
            0.33f,
        };
        //------------------------------------------------//

        private Rigidbody2D m_rigidbody2D;
        private Collider2D m_collider2D;


        //Unity Functions
        //============================================================================================================//
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            m_rigidbody2D = GetComponent<Rigidbody2D>();
            m_collider2D = GetComponent<Collider2D>();
        }

        private void FixedUpdate()
        {
            var newVelocity = m_rigidbody2D.linearVelocity;
        
            //Prevents sticking to walls when in the air, and attempting to move left or Right
            if(hittingWall == false && isGrounded)
                newVelocity.x = m_currentMoveForce * moveSpeed * Time.fixedDeltaTime;

            //If we're falling, we want to increase gravity so things don't feel floaty
            if (newVelocity.y < 0f || !isPressingJump)
                newVelocity += Physics2D.gravity * (downForceMult * Time.fixedDeltaTime);
        

            m_rigidbody2D.linearVelocity = newVelocity;
        }

        // Update is called once per frame
        private void Update()
        {
            isGrounded = ProcessGrounded();
            hittingWall = ProcessHitWall();
            ProcessMove();
            ProcessJumpV2();
        }
    
        //============================================================================================================//
    
        private bool ProcessGrounded()
        {
            const float CAST_DISTANCE = 0.1f;

            var didHit = false;
            for (var i = 0; i < CastXPositions.Length; i++)
            {
                var origin = m_rigidbody2D.position + new Vector2(CastXPositions[i], castStartYPosition);
                var hit = Physics2D.Raycast(origin, Vector2.down, CAST_DISTANCE, levelMask.value);

                if (hit && hit.collider != m_collider2D)
                {
                    didHit = true;
                }
            
                Debug.DrawRay(origin, Vector2.down * CAST_DISTANCE, didHit ? Color.green : Color.red);
            }

            return didHit;
        }
    
        private bool ProcessHitWall()
        {
            const float CAST_DISTANCE = 0.1f;

            if (xInput == 0)
                return false;

            var xStart = xInput * castStartXPosition;
            var dir = Vector2.right * xInput;
            var didHit = false;
            for (var i = 0; i < CastXPositions.Length; i++)
            {
                var origin = m_rigidbody2D.position + new Vector2(xStart, CastYPositions[i]);
                var hit = Physics2D.Raycast(origin, dir, CAST_DISTANCE, levelMask.value);

                if (hit && hit.collider != m_collider2D)
                {
                    didHit = true;
                }
            
                Debug.DrawRay(origin, dir * CAST_DISTANCE, didHit ? Color.green : Color.red);
            }

            return didHit;
        }

        private void ProcessMove()
        {
            if (Input.GetKey(moveLeftKeycode))
            {
                //If we're in the air, and we request a direction change it should accel to the new direction
                //Otherwise, if we're on the ground, move to new direction in half the time
                if (isGrounded && xInput == 1)
                    m_currentMoveForce = 0f;
            
                xInput = -1;
            
            }
            else if (Input.GetKey(moveRightKeycode))
            {
                //If we're in the air, and we request a direction change it should accel to the new direction
                //Otherwise, if we're on the ground, move to new direction in half the time
                if (isGrounded && xInput == -1)
                    m_currentMoveForce = 0f;
            
                xInput = 1;
            }
            else
            {
                xInput = 0;
            }

            //If the player has not selected any move direction, "slowly" decelerate to a stop
            if (xInput == 0)
            {
                //TODO Apply Decel
                m_currentMoveForce = Mathf.SmoothDamp(m_currentMoveForce, xInput, ref m_moveSmoothDampVelocity, moveDeceleration);

                return;
            }

            m_currentMoveForce = Mathf.SmoothDamp(m_currentMoveForce, xInput, ref m_moveSmoothDampVelocity, moveAcceleration);
        }

        /*private void ProcessJump()
        {
            //Increase Timer
            if (Input.GetKey(jumpKeycode))
            {
                //When the player presses the jump, we'll reset the value on the frame the button was pressed
                if (Input.GetKeyDown(jumpKeycode))
                    m_jumpHoldTimer = 0f;

                //Otherwise, start Charging
                m_jumpHoldTimer = Math.Clamp(m_jumpHoldTimer + Time.deltaTime, 0f, jumpMaxHoldTime);
            }
        
            //Wait for the jump to have been let go
            if (Input.GetKeyUp(jumpKeycode) == false)
                return;

            //if(isGrounded)
            //    m_currentMoveForce *= 0.5f;
        
            //If we jump while in the air, resetting YVelocity prevents fighting the current downforce
            m_rigidbody2D.linearVelocityY = 0f;

            var jumpMult = m_jumpHoldTimer / jumpMaxHoldTime;
            //We want the jump force to be dependent on how long the player has been holding the jump button
            m_rigidbody2D.AddForceY(jumpForce * jumpMult);

            //To avoid spamming bubbles, only launch them if the button was held for at least 50% of the required time
            if (jumpMult > 0.5f)
                DidJump?.Invoke(new Vector2(xInput, 1f));
        
        }*/
        private void ProcessJumpV2()
        {
            isPressingJump = Input.GetKey(jumpKeycode);
            
            //Increase Timer
            if (Input.GetKeyDown(jumpKeycode) == false)
            {
                m_jumpHoldTimer = Math.Clamp(m_jumpHoldTimer + Time.deltaTime, 0f, jumpMaxHoldTime);
                return;
            }
            
            //Check if there's a bubble available
            if (m_jumpHoldTimer < jumpMaxHoldTime * 0.6f)
                return;
            
            //If we jump while in the air, resetting YVelocity prevents fighting the current downforce
            m_rigidbody2D.linearVelocityY = 0f;

            var jumpMult = m_jumpHoldTimer / jumpMaxHoldTime;
            //We want the jump force to be dependent on how long the player has been holding the jump button
            m_rigidbody2D.AddForceY(jumpForce * jumpMult);

            //To avoid spamming bubbles, only launch them if the button was held for at least 50% of the required time
            if (jumpMult > 0.5f && !isGrounded)
                DidJump?.Invoke(new Vector2(xInput, 1f));
            
            m_rigidbody2D.linearVelocityX = m_currentMoveForce * moveSpeed * Time.fixedDeltaTime;

            m_jumpHoldTimer = 0f;
        }

        public void ExternalJump()
        {
            //If we jump while in the air, resetting YVelocity prevents fighting the current downforce
            m_rigidbody2D.linearVelocityY = 0f;

            var jumpMult = m_jumpHoldTimer / jumpMaxHoldTime;
            //We want the jump force to be dependent on how long the player has been holding the jump button
            m_rigidbody2D.AddForceY(jumpForce * jumpMult);
        }
    
    }
}
