using System.Collections.Generic;
using System.Linq;
using GameInput;
using UnityEngine;

namespace GGJ.BubbleFall
{

    // Class that handles either dropping a bubble in a direction + force
    // or a hold + charge throw

    public class BubbleThrower : MonoBehaviour
    {
        [SerializeField]
        private Bubble bubblePrefab;

        // Input vars
        private bool _boostPressed;
        private bool _chargePressed;
        private bool _lastFrameBoostPressed;
        private bool _lastFrameChargePressed;

        // Plane the character exists on -- use this for mouse cursor intersections
        private Plane _xyPlane = new Plane(Vector3.forward, Vector3.zero);
        private Camera m_mainCamera;

        [SerializeField] private float throwCooldown = 0.1f;
        [SerializeField] private float boostCooldown = 0.1f;
        [SerializeField] private int maxBoostCount = 2;
        private int _boostAvailable;

        private float _throwCooldownTimer = 0f;
        private float _boostCooldownTimer = 0f;

        [SerializeField] private float maxChargeTime = 2f;
        [SerializeField] private float minChargeVel = 2f;
        [SerializeField] private float maxChargeVel = 10f;
        private float _chargeTimer = 0f;

        [SerializeField] private float boostVelocity = 2f;
        [SerializeField] private float boostGap = 0.5f;

        private PlayerMovementV2 _playerMovement;
        private Rigidbody2D _throwerRb;
        private CaptiveGatherController _captiveGather;
        private Animator anim;
        // Track active bubbles (destroy on reset)
        private List<Bubble> _bubbleList = new List<Bubble>();

        //Unity Functions
        //============================================================================================================//

        private void OnEnable()
        {
            GameInputDelegator.OnLeftClick += OnLeftClick;
            GameInputDelegator.OnJumpPressed += OnJumpPressed;
        }

        private void Start()
        {
            m_mainCamera = Camera.main;
            _playerMovement = GetComponent<PlayerMovementV2>();
            _throwerRb = GetComponent<Rigidbody2D>();
            _captiveGather = GetComponent<CaptiveGatherController>();
            anim = GetComponent<Animator>();
        }

        private void OnDisable()
        {
            GameInputDelegator.OnLeftClick -= OnLeftClick;
            GameInputDelegator.OnJumpPressed -= OnJumpPressed;
        }

        private void Update()
        {
            // update timers
            _chargeTimer += Time.deltaTime;
            _throwCooldownTimer -= Time.deltaTime;
            _boostCooldownTimer -= Time.deltaTime;

            var boostPressedThisFrame = !_lastFrameBoostPressed && _boostPressed;
            var chargePressedThisFrame = !_lastFrameChargePressed && _chargePressed;

            // Reset boost count on touching ground
            if (_playerMovement.IsGrounded)
                _boostAvailable = maxBoostCount;

            if (_boostCooldownTimer <= 0f)
            {
                // jump is boost bubble
                if (boostPressedThisFrame && !_playerMovement.IsGrounded && _boostAvailable > 0)
                {
                    DoBubbleBoost();
                }
            }

            // throw system is ready for another deploy
            if (_throwCooldownTimer <= 0f)
            {

                // left click is charge throw
                if (chargePressedThisFrame)
                {
                    _chargeTimer = 0f;
                }

                // charge was released this frame
                if (_lastFrameChargePressed && !_chargePressed)
                {
                    var strength = Mathf.Clamp01(_chargeTimer / throwCooldown);
                    DoBubbleThrow(strength);
                }

            }

            _lastFrameBoostPressed = _boostPressed;
            _lastFrameChargePressed = _chargePressed;
        }

        private Vector2 GetMouseDirection()
        {
            var ray = m_mainCamera.ScreenPointToRay(Input.mousePosition);
            if (_xyPlane.Raycast(ray, out float distance))
            {
                var hitPoint = ray.GetPoint(distance);
                var dir = hitPoint - transform.position;
                return new Vector2(dir.x, dir.y).normalized;
            }

            return Vector2.zero;
        }


        private void DoBubbleBoost()
        {
            var position = _playerMovement.bubbleDropLocation + Vector2.down * boostGap;
            LaunchBubble(Vector2.down * 0.1f, position);
            // Apply force backwards to launcher
            // _playerMovement.AddExternalVel(Vector2.up * boostVelocity);
            _boostCooldownTimer = boostCooldown;
            _boostAvailable -= 1;
        }

        private void DoBubbleThrow(float strength)
        {

            var vel = (maxChargeVel - minChargeVel) * strength + minChargeVel;

            Debug.Log("Bubble throw");
            // TODO -- adjust charge velocity here
            // for now we always throw at max force
            var dir = GetMouseDirection();
            var position = _playerMovement.bubbleThrowLocation;
            anim.SetBool("casting", true);
            if (_captiveGather.TotalCaptives == 0)
            {
                LaunchBubble(dir * vel, position);
            }
            else
            {
                ThrowCaptive(dir * vel, position);

            }
            _throwCooldownTimer = throwCooldown;
        }

        private Transform ThrowCaptive(Vector2 velocity, Vector2 position)
        {
            // Shoot projectile
            var captive = _captiveGather.RequestCaptive();
            var bubble = captive.bubble;
            bubble.gameObject.SetActive(true);
            bubble.m_worldStartPosition = transform.position;
            var rb = bubble.transform.GetComponent<Rigidbody2D>();
            // var c2d = bubble.transform.GetComponent<Collider2D>();
            // var thisCollider = GetComponent<Collider2D>();
            // Physics2D.IgnoreCollision(thisCollider, c2d);

            // rb.bodyType = RigidbodyType2D.Dynamic;
            // c2d.enabled = true;

            bubble.transform.position = position;
            rb.linearVelocity = velocity;

            return bubble.transform;
        }

        private void LaunchBubble(Vector2 velocity, Vector2 position)
        {
            var bubble = Instantiate(bubblePrefab, position, Quaternion.identity);
            _bubbleList.Add(bubble);
            bubble.Init(velocity);
        }

        public void Reset()
        {
            while (_bubbleList.Count > 0)
            {
                var bubble = _bubbleList[0];
                _bubbleList.RemoveAt(0);
                if (bubble)
                    Destroy(bubble.gameObject);
            }
        }

        //Callbacks
        //============================================================================================================//

        private void OnLeftClick(bool pressed)
        {
            _chargePressed = pressed;
        }
        private void OnJumpPressed(bool pressed)
        {
            _boostPressed = pressed;
        }

    }
}