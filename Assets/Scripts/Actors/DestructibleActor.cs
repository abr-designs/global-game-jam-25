using UnityEngine;

namespace GGJ.BubbleFall
{
    public class DestructibleActor : MonoBehaviour, ICanBeCaptured
    {
        public bool IsCaptured => false;

        //============================================================================================================//

        public GameObject Capture()
        {
            Destroy(gameObject);

            return null;
        }

    }
}
