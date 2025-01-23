using System;
using GameInput;
using NaughtyAttributes;
using UnityEngine;

namespace Protoype.Alex_Side_Scroller
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : ActorBase, ICanBeBubbled
    {
        public static event Action<Vector2> DidJump;
    
        [Header("Movement")]
        //------------------------------------------------//
        [SerializeField, Min(0f)]private float moveSpeed;

        [SerializeField, Min(0f)]private float moveAcceleration;
        [SerializeField, Min(0f)]private float inAirMoveAcceleration;
        [SerializeField, Min(0f)]private float moveDeceleration;

        private float m_currentMoveForce;
        private float m_moveSmoothDampVelocity;

        [Header("Jump")]
        //------------------------------------------------//
        [SerializeField, Min(0)]
        private int jumpLimit;
        [SerializeField, ReadOnly]
        private int m_currentJumpCount;
        [SerializeField, Min(0f)]private float jumpForce;
        [SerializeField, Min(0f)]private float jumpMaxHoldTime;
        private float m_jumpHoldTimer;
        [SerializeField, Min(1f)]private float downForceMult = 2f;
        
   
        [Header("Inputs")]
        //------------------------------------------------//
        [SerializeField]private KeyCode moveLeftKeycode = KeyCode.A;
        [SerializeField]private KeyCode moveRightKeycode = KeyCode.D;
        [SerializeField]private KeyCode jumpKeycode = KeyCode.Space;
        private bool m_isPressingJump;
        private bool m_queueJump;

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

        //Actor Base Overrides
        //============================================================================================================//
        
        protected override void OnStart()
        {
            //Make sure that the charge time for the bubble matches that of the player
            var bubbleLauncher = GetComponent<BubbleLauncher>();
            if (bubbleLauncher)
                bubbleLauncher.chargeTime = jumpMaxHoldTime;
        }
        
        //Unity Functions
        //============================================================================================================//

        private void OnEnable()
        {
            GameInputDelegator.OnMovementChanged += OnMovementChanged;
            GameInputDelegator.OnJumpPressed += OnJumpPressed;
        }

        private void FixedUpdate()
        {
            var newVelocity = Rigidbody2D.linearVelocity;
        
            //Prevents sticking to walls when in the air, and attempting to move left or Right
            if(hittingWall == false /*&& isGrounded*/)
                newVelocity.x = m_currentMoveForce * moveSpeed * Time.fixedDeltaTime;

            //If we're falling, we want to increase gravity so things don't feel floaty
            if (newVelocity.y < 0f || !m_isPressingJump)
                newVelocity += Physics2D.gravity * (downForceMult * Time.fixedDeltaTime);

            Rigidbody2D.linearVelocity = newVelocity;
        }

        // Update is called once per frame
        private void Update()
        {
            isGrounded = ProcessIsGrounded();
            hittingWall = ProcessIsTouchingWall();
            ProcessMove();

            if (isGrounded && !m_isPressingJump)
                m_currentJumpCount = 0;
            
            ProcessJumpV2();
        }

        private void OnDisable()
        {
            GameInputDelegator.OnMovementChanged -= OnMovementChanged;
            GameInputDelegator.OnJumpPressed -= OnJumpPressed;
        }
    
        //============================================================================================================//
    
        private bool ProcessIsGrounded()
        {
            const float CAST_DISTANCE = 0.1f;

            var didHit = false;
            for (var i = 0; i < CastXPositions.Length; i++)
            {
                var origin = Rigidbody2D.position + new Vector2(CastXPositions[i], castStartYPosition);
                var hit = Physics2D.Raycast(origin, Vector2.down, CAST_DISTANCE, levelMask.value);

                if (hit && hit.collider != Collider2D)
                {
                    didHit = true;
                }
            
                Debug.DrawRay(origin, Vector2.down * CAST_DISTANCE, didHit ? Color.green : Color.red);
            }

            return didHit;
        }
    
        private bool ProcessIsTouchingWall()
        {
            const float CAST_DISTANCE = 0.1f;

            if (xInput == 0)
                return false;

            var xStart = xInput * castStartXPosition;
            var dir = Vector2.right * xInput;
            var didHit = false;
            for (var i = 0; i < CastXPositions.Length; i++)
            {
                var origin = Rigidbody2D.position + new Vector2(xStart, CastYPositions[i]);
                var hit = Physics2D.Raycast(origin, dir, CAST_DISTANCE, levelMask.value);

                if (hit && hit.collider != Collider2D)
                {
                    didHit = true;
                }
            
                Debug.DrawRay(origin, dir * CAST_DISTANCE, didHit ? Color.green : Color.red);
            }

            return didHit;
        }

        private void ProcessMove()
        {
            //If the player has not selected any move direction, "slowly" decelerate to a stop
            if (xInput == 0)
            {
                //Don't apply the same Decel while in the air
                if(isGrounded == false)
                    return;
                
                //Apply Decel
                m_currentMoveForce = Mathf.SmoothDamp(m_currentMoveForce, xInput, ref m_moveSmoothDampVelocity, moveDeceleration);
                return;
            }

            //Use the alternative acceleration if we're in the air
            m_currentMoveForce = Mathf.SmoothDamp(
                m_currentMoveForce, 
                xInput, 
                ref m_moveSmoothDampVelocity,
                isGrounded ? moveAcceleration : inAirMoveAcceleration);
        }

        //FIXME This version of ProcessJump() requires you to charge the bubble to jump when letting go
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
            //Increase Timer
            if (m_queueJump == false)
            {
                m_jumpHoldTimer = Math.Clamp(m_jumpHoldTimer + Time.deltaTime, 0f, jumpMaxHoldTime);
                return;
            }
            
            //If we jump while in the air, resetting YVelocity prevents fighting the current downforce
            Rigidbody2D.linearVelocityY = 0f;

            var jumpMult = m_jumpHoldTimer / jumpMaxHoldTime;
            //We want the jump force to be dependent on how long the player has been holding the jump button
            Rigidbody2D.AddForceY(jumpForce * jumpMult);

            //To avoid spamming bubbles, only launch them if the button was held for at least 50% of the required time
            if (jumpMult > 0.5f && !isGrounded)
                DidJump?.Invoke(new Vector2(xInput, 1f));
            
            //FIXME If you want to change direction via jump, you'll need this line below
            //Rigidbody2D.linearVelocityX = m_currentMoveForce * moveSpeed * Time.fixedDeltaTime;

            m_jumpHoldTimer = 0f;
            m_queueJump = false;
            m_currentJumpCount++;
        }

        public void ExternalJump()
        {
            //If we jump while in the air, resetting YVelocity prevents fighting the current downforce
            Rigidbody2D.linearVelocityY = 0f;

            var jumpMult = m_jumpHoldTimer / jumpMaxHoldTime;
            //We want the jump force to be dependent on how long the player has been holding the jump button
            Rigidbody2D.AddForceY(jumpForce * jumpMult);
        }

        //Input Callbacks
        //============================================================================================================//
        
        private void OnJumpPressed(bool pressed)
        {
            if (!m_isPressingJump && 
                pressed && 
                (m_jumpHoldTimer >= jumpMaxHoldTime * 0.6f)
                && (jumpLimit == 0 || m_currentJumpCount < jumpLimit))
                m_queueJump = true;
            
            m_isPressingJump = pressed;
        }

        private void OnMovementChanged(Vector2 movement)
        {
            if (movement.x < 0)
            {
                //If we're in the air, and we request a direction change it should accel to the new direction
                //Otherwise, if we're on the ground, move to new direction in half the time
                if (isGrounded && xInput == 1)
                    m_currentMoveForce = 0f;

                xInput = -1;

            }
            else if (movement.x > 0)
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
        }

    }
}
