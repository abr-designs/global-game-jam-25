using System;
using NaughtyAttributes;
using UnityEngine;

namespace Protoype.Alex
{
    [Serializable]
    public class ScaleableValue
    {
        public event Action OnValueChanged;
        
        public float Value => (baseValue + additive) * multiplier;
        
        [SerializeField]
        private float baseValue;

        [SerializeField, ReadOnly]
        private float additive;
        [SerializeField, ReadOnly]
        private float multiplier = 1f;

        public void Add(float amount)
        {
            additive += amount;
            OnValueChanged?.Invoke();
        }

        public void Subtract(float amount)
        {
            additive -= amount;
            OnValueChanged?.Invoke();
        }

        public void Multiply(float amount){
            multiplier *= amount;
            OnValueChanged?.Invoke();
        }
        public void Divide(float amount)
        {
            multiplier /= amount;
            OnValueChanged?.Invoke();
        }
    }
}