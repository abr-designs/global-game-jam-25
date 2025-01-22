using UnityEngine;

namespace Protoype.Alex_Side_Scroller
{
    public class Bubble : MonoBehaviour
    {
        [SerializeField, Min(1f)]
        private float decelMult = 1f;

        [SerializeField, Min(0.001f)]
        private float lifeTime = 1f;

        private float m_lifeTimer;
        
        private Vector2 m_velocity;
        private void Update()
        {
            if (m_lifeTimer <= 0f)
            {
                Destroy(gameObject);
                return;
            }

            m_lifeTimer -= Time.deltaTime;
            
            ProcessMove();
        }
        //============================================================================================================//

        public void Init(Vector2 velocity)
        {
            m_velocity = velocity;
            m_lifeTimer = lifeTime;
        }

        private void ProcessMove()
        {
            var newPosition = (Vector2)transform.position;
            
            newPosition += m_velocity * Time.deltaTime;

            //Slow down the Bubble
            m_velocity -= m_velocity * (Time.deltaTime * decelMult);

            transform.position = newPosition;
        }
    }
}