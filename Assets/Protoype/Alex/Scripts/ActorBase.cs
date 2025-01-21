using System;
using UnityEngine;
using Utilities.Debugging;

namespace Protoype.Alex
{
    public abstract class ActorBase : MonoBehaviour
    {
        public string targetTag;
        [Min(0.01f)]
        public float collisionRadius = 0.2f;
        
        [Header("Health")]
        public ScaleableValue maxHealth;
        private float m_currentHealth;
        
        [Header("Speed")]
        public ScaleableValue moveSpeed;
        public ScaleableValue size;

        [Header("Shooting")]
        public ScaleableValue shootCooldown;

        protected float Cooldown;
        public ScaleableValue shootDamage;
        public ScaleableValue bulletSpeed;
        public ScaleableValue bulletSize;
        public Vector3[] localShootDirections;

        //Unity Functions
        //============================================================================================================//

        private void OnEnable()
        {
            GameController.RegisterActor(this);
        }

        private void OnDisable()
        {
            GameController.DeRegisterActor(this);
        }

        //============================================================================================================//`


        public void Damage(float amount)
        {
            m_currentHealth -= amount;

            if (m_currentHealth > 0f)
                return;

            Debug.Log($"<color=red>{gameObject.name} is dead!</color>", gameObject);
            Destroy(gameObject);
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Draw.Circle(transform.position, Color.yellow, collisionRadius);
        }

#endif
    }
}
