using UnityEngine;

namespace Protoype.Alex_Side_Scroller
{
    public class BubbleLauncher : MonoBehaviour
    {
        [SerializeField]
        private Bubble bubblePrefab;

        [SerializeField, Min(0f)]
        private float speed;

        //Unity Functions
        //============================================================================================================//
        
        private void OnEnable()
        {
            PlayerController.DidJump += OnJumped;
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
        }
    }
}