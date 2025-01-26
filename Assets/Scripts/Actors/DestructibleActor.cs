using UnityEngine;

namespace GGJ.BubbleFall
{
    public class DestructibleActor : MonoBehaviour, ICanBeCaptured
    {
        public bool IsCaptured => false;


        public Bubble bubble => _bubble;
        private Bubble _bubble;

        //============================================================================================================//

        public GameObject Capture(Bubble bubble)
        {
            _bubble = bubble;
            Destroy(gameObject);

            return null;
        }

    }
}
