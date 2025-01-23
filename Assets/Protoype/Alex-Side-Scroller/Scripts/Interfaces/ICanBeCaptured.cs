using UnityEngine;

namespace Protoype.Alex_Side_Scroller
{
    public interface ICanBeCaptured : ICanBeBubbled
    {
        Transform transform { get; }
        bool IsCaptured { get; }

        GameObject Capture();
    }
    

}