using System;
using UnityEngine;
using Utilities.Tweening;

namespace Protoype.Alex
{
    public class PlayerController : ActorBase
    {
        private Camera m_mainCamera;

        //Unity Functions
        //============================================================================================================//

        protected override void OnEnable()
        {
            base.OnEnable();
            size.OnValueChanged += OnSizeChanged;
        }

        protected override void Start()
        {
            base.Start();
            
            m_mainCamera = Camera.main;
            
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            size.OnValueChanged -= OnSizeChanged;
        }



        private void Update()
        {
            FaceMouse();

            var inputs = ProcessInput();
            if(inputs != Vector2.zero)
                Move(transform, moveSpeed.Value, inputs);

            if (Cooldown > 0f)
                Cooldown -= Time.deltaTime;
            else
                ShouldShoot();

        }
        
        //Movement Functions
        //============================================================================================================//

        #region Movement Functions

        private void FaceMouse()
        {
            var mousePosition = Input.mousePosition;
            var mouseWorldSpacePosition = m_mainCamera.ScreenToWorldPoint(mousePosition);
            var dir = Vector3.ProjectOnPlane(mouseWorldSpacePosition - transform.position, Vector3.forward);

            transform.up = dir.normalized;
        }

        private static Vector2 ProcessInput()
        {
            var outValue = Vector2.zero;
            Span<bool> keys = stackalloc bool[]
            {
                Input.GetKey(KeyCode.W),
                Input.GetKey(KeyCode.A),
                Input.GetKey(KeyCode.S),
                Input.GetKey(KeyCode.D)
            };
            
            if (keys[1])
                outValue.x = -1f;
            else if (keys[3])
                outValue.x = 1f;
            else if (!keys[1] && !keys[3])
            {
                outValue.x = 0f;
            }
            
            if (keys[0])
                outValue.y = 1f;
            else if (keys[2])
                outValue.y = -1f;
            else if (!keys[0] && !keys[2])
            {
                outValue.y = 0f;
            }

            return outValue;
        }

        private static void Move(Transform target, float speed, Vector2 directions)
        {
            var position = target.position;
            position += ((Vector3.right * directions.x) + (Vector3.up * directions.y)) * (speed * Time.deltaTime);

            target.position = position;
        }

        #endregion //Movement Functions

        //Callbacks
        //============================================================================================================//
        
        private void OnSizeChanged()
        {
            transform.TweenScaleTo(size.Value * startingScale, 0.5f, CURVE.EASE_OUT);
        }

        //Shooting Functions
        //============================================================================================================//

        private void ShouldShoot()
        {
            if (Input.GetKey(KeyCode.Mouse0) == false)
                return;

            Cooldown = shootCooldown.Value;
            Bullet.Create(TagHelper.ENEMY, 
                transform.up, 
                bulletSpeed.Value, 
                bulletSize.Value, 
                shootDamage.Value, 
                transform.TransformPoint(Vector3.up));
            
        }
    }
}
