using System;
using UnityEngine;
using Utilities.Tweening;

namespace Protoype.Alex_Side_Scroller
{
    public class BubbleLauncher : MonoBehaviour
    {
        [SerializeField]
        private Bubble bubblePrefab;

        [SerializeField, Min(0f)]
        private float speed;

        [SerializeField]
        private Transform bubbleSpriteTransform;
        [SerializeField]
        private float chargeTime;

        private Vector3 m_startingScale;

        //Unity Functions
        //============================================================================================================//
        
        private void OnEnable()
        {
            PlayerController.DidJump += OnJumped;
        }

        private void Start()
        {
            m_startingScale = bubbleSpriteTransform.localScale;
            bubbleSpriteTransform.localScale = Vector3.zero;
            bubbleSpriteTransform.TweenScaleTo(m_startingScale, chargeTime, CURVE.EASE_IN_OUT);
        }

        private void OnDisable()
        {
            PlayerController.DidJump -= OnJumped;
        }
        
        //Callbacks
        //============================================================================================================//
        
        private void OnJumped(Vector2 direction)
        {
           var bubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity);
           direction.y *= -1f;
           bubble.Init(direction * speed);
           
           bubbleSpriteTransform.localScale = Vector3.zero;
           bubbleSpriteTransform.TweenScaleTo(m_startingScale, chargeTime, CURVE.EASE_IN_OUT);
        }
    }
}