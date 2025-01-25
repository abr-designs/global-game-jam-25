using UnityEngine;

namespace GGJ.BubbleFall
{
    public interface ICanBeReleased : ICanBeBubbled
    {
        Transform transform { get; }
        bool IsCaptured { get; }

        float MaxIdleTime { get; }

        void Release();
    }
}