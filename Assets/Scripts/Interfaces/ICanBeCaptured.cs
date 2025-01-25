using UnityEngine;

namespace GGJ.BubbleFall
{
    public interface ICanBeCaptured : ICanBeBubbled
    {
        Transform transform { get; }
        bool IsCaptured { get; }

        GameObject Capture();
    }


}