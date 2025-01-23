using UnityEngine;

namespace Protoype.Alex_Side_Scroller
{
    public interface ICanBeCaptured
    {
        Transform transform { get; }
        bool IsCaptured { get; }
        float MaxIdleTime { get; }

        GameObject Capture();
    }
}