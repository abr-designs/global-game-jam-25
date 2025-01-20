using System;
using NaughtyAttributes;
using UnityEngine;

namespace Protoype.Alex
{
    [Serializable]
    public class ScaleableValue
    {
        public float Value => (baseValue + additive) * multiplier;
        
        [SerializeField]
        private float baseValue;

        [SerializeField, ReadOnly]
        private float additive;
        [SerializeField, ReadOnly]
        private float multiplier = 1f;

        public void Add(float amount) => additive += amount;
        public void Subtract(float amount) => additive -= amount;
        
        public void Multiply(float amount) => multiplier *= amount;
        public void Divide(float amount) => multiplier /= amount;
    }
}