using UnityEngine;

namespace GGJ.BubbleFall
{
    public interface ICanBeCaptured : ICanBeBubbled
    {
        Transform transform { get; }
        bool IsCaptured { get; }

        Bubble bubble { get; }

        GameObject Capture(Bubble bubble);
    }


}