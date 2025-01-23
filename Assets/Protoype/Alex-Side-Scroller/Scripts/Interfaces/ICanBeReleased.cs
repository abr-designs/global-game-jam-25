using UnityEngine;

namespace Protoype.Alex_Side_Scroller
{
    public interface ICanBeReleased : ICanBeBubbled
    {
        Transform transform { get; }
        bool IsCaptured { get; }

        float MaxIdleTime { get; }

        void Release();
    }
}