using UnityEngine;

namespace Protoype.Alex_Side_Scroller.Scripts
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
