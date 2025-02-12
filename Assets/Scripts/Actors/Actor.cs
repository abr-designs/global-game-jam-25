using NaughtyAttributes;
using UnityEngine;

namespace GGJ.BubbleFall
{
    public class Actor : MonoBehaviour, ICanBeCaptured, ICanBeReleased
    {
        enum STATE
        {
            NONE,
            IDLE,
            MOVE,
            CAPTURED
        }
        public enum ATTRIBUTE
        {
            NONE,
            FROZEN,
            EXPLOSIVE,
            FIRE,
            STATIC

        }
        //ICanBeCaptured Properties
        //------------------------------------------------//
        [field: SerializeField, ReadOnly]
        public bool IsCaptured { get; private set; }
        public bool CanBeReleased => canBeReleased;
        [SerializeField]
        private bool canBeReleased;
        public float MaxIdleTime => maxIdleTime;

        private Bubble _bubble;
        Bubble ICanBeCaptured.bubble => _bubble;

        [SerializeField]
        private float maxIdleTime;

        public ATTRIBUTE attribute;

        //------------------------------------------------//

        private Rigidbody2D m_rigidbody2D;
        private Collider2D m_collider2D;

        //Unity Functions
        //============================================================================================================//

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            m_rigidbody2D = GetComponent<Rigidbody2D>();
            m_collider2D = GetComponent<Collider2D>();
        }

        //============================================================================================================//

        public GameObject Capture(Bubble bubble)
        {
            _bubble = bubble;

            // Disable physics on actor
            m_collider2D.enabled = false;
            m_rigidbody2D.simulated = false;

            // Do any custom attribute processing here

            // switch (attribute)
            // {
            //     case ATTRIBUTE.NONE:
            //         IsCaptured = true;
            //         m_rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            //         // m_rigidbody2D.linearVelocity = Vector2.zero;
            //         break;
            //     case ATTRIBUTE.FIRE:
            //         IsCaptured = true;
            //         m_rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            //         // m_rigidbody2D.freezeRotation = true;
            //         // m_rigidbody2D.gravityScale = -0.01f;
            //         // m_rigidbody2D.linearDamping = 0.1f;
            //         break;
            // }

            return gameObject;
        }
        public void Release()
        {
            throw new System.NotImplementedException();
        }
    }
}
