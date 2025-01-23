using UnityEngine;

namespace Protoype.Alex_Side_Scroller
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class ActorBase : MonoBehaviour
    {
        protected Rigidbody2D Rigidbody2D;
        protected Collider2D Collider2D;

        private void Start()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            Collider2D = GetComponent<Collider2D>();
            
            OnStart();
        }

        protected abstract void OnStart();
    }
}