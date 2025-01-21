using UnityEngine;
using Utilities.Physics;

namespace Protoype.Alex
{
    public class Enemy : ActorBase
    {
        private static PlayerController s_playerController;

        //Unity Functions
        //============================================================================================================//

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            s_playerController ??= FindObjectsByType<PlayerController>(FindObjectsSortMode.None)[0];
        }

        // Update is called once per frame
        private void Update()
        {
            //If we've hit the player, damage them, then destroy ourselves
            if (DidColliderWithPlayer())
            {
                s_playerController.Damage(1f);
                Destroy(gameObject);
                return;
            }
            
            var currentPosition = transform.position;

            var dir = (s_playerController.transform.position - currentPosition).normalized;

            currentPosition += dir * (moveSpeed.Value * Time.deltaTime);

            transform.position = currentPosition;
        }

        //============================================================================================================//

        private bool DidColliderWithPlayer()
        {
            return CollisionChecks.CircleToCircle(
                    s_playerController.transform.position,
                    s_playerController.collisionRadius,
                    transform.position,
                    collisionRadius);
        }

    }
}
