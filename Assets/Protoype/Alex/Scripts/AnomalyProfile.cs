using System;
using UnityEngine;

namespace Protoype.Alex
{
    [Serializable]
    public class AnomalyProfile
    {
        public Color32 color;
        public Affector[] affectors;
        public AnimationCurve growthCurve;
        
        [Min(0.1f)]
        public float maxSize;

        [Min(0f)]
        public float activeTime;
    }

    [Serializable]
    public struct Affector
    {
        public TARGET_STAT targetStat;
        public DELTA_TYPE deltaType;
        public float amount;

        public void Apply(ActorBase actorBase)
        {
            switch (targetStat)
            {
                case TARGET_STAT.NONE:
                    break;
                case TARGET_STAT.HEALTH:
                    ApplyAmount(actorBase.maxHealth, deltaType, amount);
                    break;
                case TARGET_STAT.MOVE_SPEED:
                    ApplyAmount(actorBase.moveSpeed, deltaType, amount);
                    break;
                case TARGET_STAT.SIZE:
                    ApplyAmount(actorBase.size, deltaType, amount);
                    break;
                case TARGET_STAT.COOLDOWN:
                    ApplyAmount(actorBase.shootCooldown, deltaType, amount);
                    break;
                case TARGET_STAT.DAMAGE:
                    ApplyAmount(actorBase.shootDamage, deltaType, amount);
                    break;
                case TARGET_STAT.BULLET_SPEED:
                    ApplyAmount(actorBase.bulletSpeed, deltaType, amount);
                    break;
                case TARGET_STAT.BULLET_SIZE:
                    ApplyAmount(actorBase.bulletSize, deltaType, amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void Remove(ActorBase actorBase)
        {
            switch (targetStat)
            {
                case TARGET_STAT.NONE:
                    break;
                case TARGET_STAT.HEALTH:
                    RemoveAmount(actorBase.maxHealth, deltaType, amount);
                    break;
                case TARGET_STAT.MOVE_SPEED:
                    RemoveAmount(actorBase.moveSpeed, deltaType, amount);
                    break;
                case TARGET_STAT.SIZE:
                    RemoveAmount(actorBase.size, deltaType, amount);
                    break;
                case TARGET_STAT.COOLDOWN:
                    RemoveAmount(actorBase.shootCooldown, deltaType, amount);
                    break;
                case TARGET_STAT.DAMAGE:
                    RemoveAmount(actorBase.shootDamage, deltaType, amount);
                    break;
                case TARGET_STAT.BULLET_SPEED:
                    RemoveAmount(actorBase.bulletSpeed, deltaType, amount);
                    break;
                case TARGET_STAT.BULLET_SIZE:
                    RemoveAmount(actorBase.bulletSize, deltaType, amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ApplyAmount(ScaleableValue scaleableValue, DELTA_TYPE deltaType, float amount)
        {
            switch (deltaType)
            {
                case DELTA_TYPE.NONE:
                    break;
                case DELTA_TYPE.ADD:
                    scaleableValue.Add(amount);
                    break;
                case DELTA_TYPE.MULT:
                    scaleableValue.Multiply(amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(deltaType), deltaType, null);
            }
        }
        
        private void RemoveAmount(ScaleableValue scaleableValue, DELTA_TYPE deltaType, float amount)
        {
            switch (deltaType)
            {
                case DELTA_TYPE.NONE:
                    break;
                case DELTA_TYPE.ADD:
                    scaleableValue.Subtract(amount);
                    break;
                case DELTA_TYPE.MULT:
                    scaleableValue.Divide(amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(deltaType), deltaType, null);
            }
        }
    }

    public enum TARGET_STAT
    {
        NONE,
        HEALTH,
        MOVE_SPEED,
        SIZE,
        COOLDOWN,
        DAMAGE,
        BULLET_SPEED,
        BULLET_SIZE,
        
    }

    public enum DELTA_TYPE
    {
        NONE,
        ADD,
        MULT
    }
}