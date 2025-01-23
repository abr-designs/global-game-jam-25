using NaughtyAttributes;
using UnityEngine;

namespace Protoype.Alex_Side_Scroller
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

        //ICanBeCaptured Properties
        //------------------------------------------------//
        [field: SerializeField, ReadOnly]
        public bool IsCaptured { get; private set; }
        public bool CanBeReleased => canBeReleased;
        [SerializeField]
        private bool canBeReleased;
        public float MaxIdleTime => maxIdleTime;
        [SerializeField]
        private float maxIdleTime;

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

        public GameObject Capture()
        {
            IsCaptured = true;
            m_rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            m_rigidbody2D.gravityScale = 0.05f;
            
            return gameObject;
        }

        public void Release()
        {
            throw new System.NotImplementedException();
        }
    }
}
