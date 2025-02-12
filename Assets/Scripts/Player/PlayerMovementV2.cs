using System;
using GameInput;
using UnityEngine;
using Utilities.Debugging;

namespace GGJ.BubbleFall
{

    // Based on the controller from Sasquatch Studios
    // https://www.youtube.com/watch?v=zHSWG05byEc

    public class PlayerMovementV2 : ActorBase, ICanBeBubbled
    {

        [Header("References")]
        public PlayerMovementStats MoveStats;
        [SerializeField] private Collider2D _feetColl;
        [SerializeField] private Collider2D _bodyColl;

        private Rigidbody2D _rb;

        // move vars (horizontal input)
        private Vector2 _moveVelocity;
        private bool _isFacingRight;

        // collision check
        private RaycastHit2D _groundHit;
        private RaycastHit2D _headHit;
        private RaycastHit2D _leftSideBodyHit;
        private RaycastHit2D _leftSideFeetHit;
        private RaycastHit2D _leftSideHeadHit;

        private RaycastHit2D _rightSideBodyHit;
        private RaycastHit2D _rightSideFeetHit;
        private RaycastHit2D _rightSideHeadHit;
        private bool _isOnWall;
        private Vector2 _wallNormal;


        private bool _isGrounded;
        public bool IsGrounded => _isGrounded;
        private bool _bumpedHead;

        // jump vars
        public float VerticalVelocity { get; private set; }
        private bool _isJumping;
        private bool _isJumpAvailable;
        private bool _isFalling;

        // jump buffer vars
        private float _jumpBufferTimer;

        // coyote time vars
        private float _coyoteTimer;

        // input
        private Vector2 _lastFrameInput = Vector2.zero;
        private Vector2 _moveInput;
        private bool _lastFrameJumpPressed = false;
        private bool _isJumpPressed;

        // external force
        private Vector2 _externalVel = Vector2.zero;

        public Vector2 bubbleDropLocation => new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        public Vector2 bubbleThrowLocation => new Vector2(_bodyColl.bounds.center.x, _bodyColl.bounds.center.y);


        // keeping track of spawn position

        public GameObject LastSpawnLocation;


        // animation vars
        [SerializeField] private ParticleSystem dustParticleSystem;
        private Animator anim;
        [SerializeField] GameObject playerModel;

        private void OnEnable()
        {
            GameInputDelegator.OnMovementChanged += OnMovementChanged;
            GameInputDelegator.OnJumpPressed += OnJumpPressed;
        }


        private void Awake()
        {
            _isFacingRight = true;
            _rb = GetComponent<Rigidbody2D>();
            anim = GetComponentInChildren<Animator>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void OnStart()
        {

        }

        // Update is called once per frame
        void Update()
        {
            CountTimers();
            JumpInputChecks();

            _lastFrameInput = _moveInput;
            _lastFrameJumpPressed = _isJumpPressed;

            DoAnimations();

        }

        private void FixedUpdate()
        {
            CollisionChecks();
            VerticalPhysics();

            // Use air/ground acceleration values for horizontal velocity
            if (_isGrounded)
            {
                Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, _moveInput);
            }
            else
            {
                Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, _moveInput);
            }

        }

        private void OnDisable()
        {
            GameInputDelegator.OnMovementChanged -= OnMovementChanged;
            GameInputDelegator.OnJumpPressed -= OnJumpPressed;
        }


        #region Movement

        // Horizontal movement
        private void Move(float acceleration, float deceleration, Vector2 moveInput)
        {

            // Apply any external velocity this frame
            if (Mathf.Abs(_externalVel.x) > 0f)
            {
                Debug.Log($"{_externalVel}");
                _moveVelocity.x = _externalVel.x;
                _externalVel = new Vector2(0f, _externalVel.y);
            }

            if (moveInput != Vector2.zero)
            {
                Vector2 targetVelocity = Vector2.zero;

                // TODO -- here is where you would add run support (run button check)
                // for now the character will always be running
                targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxRunSpeed;

                _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            }
            else if (moveInput == Vector2.zero)
            {
                _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            }

            _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);

            // Apply correction for being against a wall (we need to immediately stop the velocity)
            bool leftWall = _leftSideHeadHit || _leftSideBodyHit || _leftSideHeadHit;
            bool rightWall = _rightSideHeadHit || _rightSideBodyHit || _rightSideFeetHit;
            if (rightWall && _rb.linearVelocity.x > 0f)
            {
                _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
            }
            if (leftWall && _rb.linearVelocity.x < 0f)
            {
                _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
            }

            // If character has a body above a ledge but feet below we shimmy them up
            // TODO -- this is where ledge grab would kick in
            if (!_leftSideBodyHit && _leftSideFeetHit && _moveInput.x < 0f)
            {
                _rb.position = _rb.position + Vector2.up * 0.1f;
            }

            if (!_rightSideBodyHit && _rightSideFeetHit && _moveInput.x > 0f)
            {
                _rb.position = _rb.position + Vector2.up * 0.1f;
            }

        }

        #endregion

        #region CollisionChecks

        private void GroundCheck()
        {
            Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
            Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, MoveStats.GroundDetectionRayLength);
            _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats.GroundDetectionRayLength, MoveStats.GroundLayer);

            _isGrounded = _groundHit.collider != null;
            #region DebugViz
            Color rayColor;
            if (_isGrounded)
            {
                rayColor = Color.green;
            }
            else rayColor = Color.red;

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - MoveStats.GroundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
            #endregion

        }

        private void BumpHeadCheck()
        {
            Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _bodyColl.bounds.max.y);
            Vector2 boxCastSize = new Vector2(MoveStats.HeadWidth, MoveStats.HeadDetectionRayLength);

            _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength, MoveStats.GroundLayer);
            _bumpedHead = _headHit.collider != null;

            #region HeadHitDebug
            float headWidth = MoveStats.HeadWidth;
            Color rayColor;
            if (_bumpedHead)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x + (boxCastSize.x / 2), boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y + MoveStats.HeadDetectionRayLength), Vector2.right * headWidth, rayColor);

            #endregion
        }

        private void CollisionChecks()
        {
            GroundCheck();
            BumpHeadCheck();
            WallCheck();
        }

        private void WallCheck()
        {
            Vector2 headPoint = new Vector2(_bodyColl.bounds.center.x, _bodyColl.bounds.max.y);
            Vector2 bodyPoint = new Vector2(_bodyColl.bounds.center.x, _bodyColl.bounds.center.y);
            Vector2 feetPoint = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);

            var cast_distance = _bodyColl.bounds.extents.x + 0.2f;

            // Check right side
            var dirVec = Vector2.right;
            _rightSideHeadHit = Physics2D.Raycast(headPoint, dirVec, cast_distance, MoveStats.GroundLayer);
            _rightSideBodyHit = Physics2D.Raycast(bodyPoint, dirVec, cast_distance, MoveStats.GroundLayer);
            _rightSideFeetHit = Physics2D.Raycast(feetPoint, dirVec, cast_distance, MoveStats.GroundLayer);

            // Check left side
            dirVec = Vector2.left;
            _leftSideHeadHit = Physics2D.Raycast(headPoint, dirVec, cast_distance, MoveStats.GroundLayer);
            _leftSideBodyHit = Physics2D.Raycast(bodyPoint, dirVec, cast_distance, MoveStats.GroundLayer);
            _leftSideFeetHit = Physics2D.Raycast(feetPoint, dirVec, cast_distance, MoveStats.GroundLayer);

            // Debug lines
            Debug.DrawRay(headPoint, Vector2.right * cast_distance, _rightSideHeadHit ? Color.green : Color.red);
            Debug.DrawRay(bodyPoint, Vector2.right * cast_distance, _rightSideBodyHit ? Color.green : Color.red);
            Debug.DrawRay(feetPoint, Vector2.right * cast_distance, _rightSideFeetHit ? Color.green : Color.red);
            Debug.DrawRay(headPoint, Vector2.left * cast_distance, _leftSideHeadHit ? Color.green : Color.red);
            Debug.DrawRay(bodyPoint, Vector2.left * cast_distance, _leftSideBodyHit ? Color.green : Color.red);
            Debug.DrawRay(feetPoint, Vector2.left * cast_distance, _leftSideFeetHit ? Color.green : Color.red);

        }

        #endregion

        #region InputHandlers

        private void OnMovementChanged(Vector2 moveInput)
        {
            _moveInput = moveInput;
        }

        private void OnJumpPressed(bool pressed)
        {
            _isJumpPressed = pressed;
        }

        #endregion

        #region Jump

        // Process vertical velocity
        // Includes jumps and bounces off bubbles
        private void JumpInputChecks()
        {

            // Player pressed jump button this frame -- start the jump buffer
            bool jumpPressedThisFrame = !_lastFrameJumpPressed && _isJumpPressed;
            bool jumpReleasedThisFrame = _lastFrameJumpPressed && !_isJumpPressed;
            // Jumping starts the buffer timer -- hitting ground within this timer will trigger a jump
            if (jumpPressedThisFrame)
            {
                _jumpBufferTimer = MoveStats.JumpBufferTime;
            }

            // We are currently on the ground
            if (_isGrounded)
            {
                // Reset coyote timer
                _coyoteTimer = MoveStats.JumpCoyoteTime;

                if (!_isJumping && (jumpPressedThisFrame || _jumpBufferTimer > 0f))
                {
                    DoJump();
                }

            }
            else
            {
                // Handle coyote time jump
                if (!_isJumping && _coyoteTimer > 0f && jumpPressedThisFrame)
                {
                    DoJump();
                }

            }


            // Bump head causes jump apex right away (no floating)
            if (_bumpedHead && VerticalVelocity > 0f)
            {
                VerticalVelocity = 0f;
            }


            // Landed
            if ((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0f)
            {
                _isJumping = false;
                _isFalling = false;
                VerticalVelocity = Physics2D.gravity.y;
                anim.SetBool("jumping", false);
            }

            return;
        }

        // Set velocity and animations for jump
        private void DoJump()
        {
            if (!_isJumping)
            {
                anim.Play("Jump");
                _isJumping = true;
            }
            _jumpBufferTimer = 0f;
            _moveVelocity.x = _moveVelocity.x * MoveStats.JumpHorizontalDampening;
            VerticalVelocity = MoveStats.JumpVelocity; //MoveStats.InitialJumpVelocity;
        }

        private void VerticalPhysics()
        {
            _isFalling = VerticalVelocity < 0f;

            // TODO -- play with fall state modifiers here
            // for now we just have one gravity once the play has left the ground
            if (!_isGrounded)
            {
                VerticalVelocity += Physics2D.gravity.y * MoveStats.FallGravityMultiplier * Time.fixedDeltaTime;
            }

            // Apply external velocity
            if (Mathf.Abs(_externalVel.y) > 0f)
            {
                VerticalVelocity += _externalVel.y;
                _externalVel = new Vector2(_externalVel.x, 0f);
            }
            if (VerticalVelocity < 0f)
            {
                //anim.SetBool("jumping", false);
                //anim.SetBool("falling", true);

            }
            // If we are falling we apply fall modifier
            // if (VerticalVelocity < 0f)
            // {
            //     VerticalVelocity += Physics2D.gravity.y * MoveStats.FallGravityMultiplier * Time.fixedDeltaTime;
            // }
            // If we are going up but not pressing jump we apply more gravity to shorten jump
            // else if (_rb.linearVelocityY > 0f && !_isJumpPressed)
            // {
            //     VerticalVelocity += Physics2D.gravity.y * MoveStats.FallGravityMultiplier * Time.fixedDeltaTime;
            // }
            // else
            // {
            //     VerticalVelocity += Physics2D.gravity.y * Time.fixedDeltaTime;
            // }

            // clamp fall speed
            // TODO -- put 50f into max upwards velocity setting?
            VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 50f);
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, VerticalVelocity);

        }

        #endregion 

        #region Timers

        private void CountTimers()
        {
            _jumpBufferTimer -= Time.deltaTime;
            if (!_isGrounded)
            {
                _coyoteTimer -= Time.deltaTime;
            }
            else
            {
                _coyoteTimer = MoveStats.JumpCoyoteTime;
            }
        }

        #endregion

        // Add speed from a source outside the input
        public void AddExternalVel(Vector2 vel)
        {
            Debug.Log($"Adding external vel {vel}");
            _externalVel += vel;
        }

        // called when a bubble detects it is overlapping a player
        public void OnBubbleCollision(Bubble bubble, float radius)
        {
            var playerIsAbove = _feetColl.bounds.min.y > bubble.transform.position.y;
            if (playerIsAbove)
            {
                // Move player to top of bubble
                //_rb.position = new Vector2(bubble.transform.position.x, bubble.transform.position.y) + Vector2.up * (radius + 0.05f);
                // TODO -- get bounce value from bubble
                //AddExternalVel(Vector2.up * MoveStats.JumpVelocity);
                VerticalVelocity = MoveStats.JumpVelocity;
                // Debug.Log($"Player/Bubble collision! -- {playerIsAbove}");
            }
        }

        #region Effects/Animations

        private void DoAnimations()
        {
            if (Mathf.Abs(_moveInput.x) > 0 && _isGrounded)
            {
                WalkAnimation(true);
                var emission = dustParticleSystem.emission;
                emission.enabled = true;

            }
            else
            {
                WalkAnimation(false);
                var emission = dustParticleSystem.emission;
                emission.enabled = false;
            }

            if (_moveInput.x > 0)
            {
                playerModel.transform.forward = Vector3.right + Vector3.down * .2f;
            }
            else if (_moveInput.x < 0)
            {
                playerModel.transform.forward = Vector3.left + Vector3.down * .2f;
            }

        }

        private void WalkAnimation(bool isWalking)
        {
            bool animatorIsWalking = anim.GetCurrentAnimatorStateInfo(0).IsName("Walking");

            if (isWalking)
            {
                anim.SetBool("walking", true);
                if (!animatorIsWalking)
                    anim.Play("Walking");
            }
            else
            {
                anim.SetBool("walking", false);
            }
        }

        #endregion

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            var vel = _rb == null ? Vector2.zero : _rb.linearVelocity;
            Draw.Label(transform.position, $"Grounded: {_isGrounded} Jump: {_isJumping} \n Velocity: {vel}");

        }

#endif
    }

}