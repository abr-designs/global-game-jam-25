using Unity.VisualScripting;
using UnityEngine;

namespace GGJ.BubbleFall
{
    // Class for an entity that does damage to the player
    class Trap : MonoBehaviour
    {
        public int DamageAmount = 1;

        public float KnockVelocity = 20f;

        private Collider2D _collider;

        void Start()
        {
            _collider = GetComponent<Collider2D>();
        }

        void OnCollisionStay2D(Collision2D collision)
        {
            var player = collision.collider.GetComponent<PlayerHealth>();
            if (player)
            {
                player.ReceiveDamage(DamageAmount);
                Debug.Log($"Trap collided with player!");

                // Add some knockback to the player
                var dir = player.transform.position - transform.position;
                var vel = dir.normalized * KnockVelocity;
                player.GetComponent<PlayerMovementV2>().AddExternalVel(vel);

            }

        }

    }


}