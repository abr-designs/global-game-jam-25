using System;
using GameInput;
using UnityEngine;
using Utilities.Tweening;

namespace GGJ.BubbleFall
{

    // Class that handles either dropping a bubble in a direction + force
    // or a hold + charge throw

    public class BubbleThrower : MonoBehaviour
    {
        [SerializeField]
        private Bubble bubblePrefab;

        // Input vars
        private bool _shootPressed;
        private bool _chargePressed;
        private bool _lastFrameShootPressed;
        private bool _lastFrameChargePressed;

        // Plane the character exists on -- use this for mouse cursor intersections
        private Plane _xyPlane = new Plane(Vector3.forward, Vector3.zero);
        private Camera m_mainCamera;

        [SerializeField] private float throwCooldown = 0.1f;
        [SerializeField] private float boostCooldown = 0.1f;
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

        //Unity Functions
        //============================================================================================================//

        private void OnEnable()
        {
            GameInputDelegator.OnLeftClick += OnLeftClick;
            GameInputDelegator.OnRightClick += OnRightClick;
        }

        private void Start()
        {
            m_mainCamera = Camera.main;
            _playerMovement = GetComponent<PlayerMovementV2>();
            _throwerRb = GetComponent<Rigidbody2D>();
        }

        private void OnDisable()
        {
            GameInputDelegator.OnLeftClick -= OnLeftClick;
            GameInputDelegator.OnRightClick -= OnRightClick;
        }

        private void Update()
        {
            // update timers
            _chargeTimer += Time.deltaTime;
            _throwCooldownTimer -= Time.deltaTime;
            _boostCooldownTimer -= Time.deltaTime;

            var shootPressedThisFrame = !_lastFrameShootPressed && _shootPressed;
            var chargePressedThisFrame = !_lastFrameChargePressed && _chargePressed;

            if (_boostCooldownTimer <= 0f)
            {
                // left click is boost bubble
                if (shootPressedThisFrame)
                {
                    DoBubbleBoost();
                }
            }

            // throw system is ready for another deploy
            if (_throwCooldownTimer <= 0f)
            {

                // right click is charge throw
                if (chargePressedThisFrame)
                {
                    _chargeTimer = 0f;
                }

                // charge was released this frame
                if (_lastFrameChargePressed && !_chargePressed)
                {
                    var strength = Mathf.Clamp01(_chargeTimer / throwCooldown);
                    DoBubbleThrow(_chargeTimer / throwCooldown);
                }

            }

            _lastFrameShootPressed = _shootPressed;
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
            _throwCooldownTimer = throwCooldown;
        }

        private void DoBubbleThrow(float strength)
        {

            var vel = (maxChargeVel - minChargeVel) * strength + minChargeVel;

            Debug.Log("Bubble throw");
            // TODO -- adjust charge velocity here
            // for now we always throw at max force
            var dir = GetMouseDirection();
            var position = _playerMovement.bubbleThrowLocation;
            LaunchBubble(dir * vel, position);
            _throwCooldownTimer = throwCooldown;
        }

        private void LaunchBubble(Vector2 velocity, Vector2 position)
        {
            var bubble = Instantiate(bubblePrefab, position, Quaternion.identity);
            bubble.Init(velocity);
        }

        //Callbacks
        //============================================================================================================//

        private void OnLeftClick(bool pressed)
        {
            _shootPressed = pressed;
        }
        private void OnRightClick(bool pressed)
        {
            _chargePressed = pressed;
        }

    }
}