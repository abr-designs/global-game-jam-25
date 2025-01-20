using UnityEngine;

namespace Protoype.Alex
{
    public class Bullet : MonoBehaviour
    {
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

        private bool DidHitTarget(string targetTag, out ActorBase actorBase)
        {
            actorBase = null;
            return false;
        }

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
