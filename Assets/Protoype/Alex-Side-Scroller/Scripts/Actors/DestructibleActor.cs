using UnityEngine;

namespace Protoype.Alex_Side_Scroller.Scripts
{
    public class DestructibleActor : MonoBehaviour, ICanBeCaptured
    {
        public bool IsCaptured { get; private set; }
        [SerializeField] private float maxIdleTime;

        //============================================================================================================//

        public GameObject Capture()
        {
            IsCaptured = true;
            Destroy(gameObject);

            return null;
        }

    }
}
