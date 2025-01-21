using UnityEngine;
using Utilities.Debugging;
using Utilities.Physics;

namespace Protoype.Alex
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField, Header("Collision Information")]
        private Vector2 offset;
        [SerializeField]
        private float collisionRadius;
        
        private float m_damage;
        private float m_speed;
        private Vector2 m_direction;
        private float m_size;
        private string m_target;

        //Unity Functions
        //============================================================================================================//
        
        private void Update()
        {
            if (DidHitTarget(m_target, out var hitActor))
            {
                hitActor.Damage(m_damage);
                Destroy(gameObject);
            }

            var newPosition = transform.position;
            newPosition += (Vector3)m_direction * (m_speed * Time.deltaTime);

            transform.position = newPosition;
        }

        //Bullet Detection Function
        //============================================================================================================//

        //FIXME It might be cheaper to do this all in a controller, instead of per-projectile
        private bool DidHitTarget(string targetTag, out ActorBase actorBase)
        {
            actorBase = null;
            var actors = GameController.Actors;
            var pos = transform.TransformPoint(offset);

            foreach (var actor in actors)
            {
                if(actor.targetTag.Equals(targetTag) == false)
                    continue;
                
                if (CollisionChecks.CircleToCircle(
                        actor.transform.position,
                        actor.collisionRadius,
                        pos,
                        collisionRadius) == false)
                {
                    continue;
                }

                actorBase = actor;
                
                return true;
            }
            return false;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            var pos = transform.TransformPoint(offset);
            Draw.Circle(pos, Color.yellow, collisionRadius);
        }

#endif

        //Creation Function
        //============================================================================================================//
        
        public static void Create(string target, Vector2 direction, float speed, float size, float damage, Vector2 worldPosition)
        {
            var bulletInstance = Factory.CreateBullet();
            
            bulletInstance.m_target = target;
            bulletInstance.m_direction = direction;
            bulletInstance.m_speed = speed;
            bulletInstance.m_size = size;
            bulletInstance.m_damage = damage;

            var newScale = bulletInstance.transform.localScale * size;
            bulletInstance.transform.localScale = newScale;
            
            bulletInstance.transform.position = worldPosition;
            bulletInstance.transform.up = direction.normalized;
        }
    }
}
