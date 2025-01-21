using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Debugging;
using Utilities.Physics;

namespace Protoype.Alex
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Anomaly : MonoBehaviour
    {
        [SerializeField]
        private AnomalyProfile m_activeProfile;

        private SpriteRenderer m_spriteRenderer;
        private ParticleSystem m_particleSystem;

        private List<ActorBase> m_overlappedActors;

        public float Radius;

        //Unity Functions
        //============================================================================================================//
        
        private void Start()
        {
            m_particleSystem ??= GetComponentInChildren<ParticleSystem>();
            m_spriteRenderer ??= GetComponent<SpriteRenderer>();
            m_overlappedActors = new List<ActorBase>();

            Radius = transform.localScale.x / 2f;
        }

        private void Update()
        {
            CheckForCollisions();
        }

        private void OnDestroy()
        {
            foreach (var overlappedActor in m_overlappedActors)
            {
                RemoveAffectors(overlappedActor);
            }
        }

        //Anomaly Functions
        //============================================================================================================//

        public void Setup(AnomalyProfile anomalyProfile)
        {
            m_spriteRenderer ??= GetComponent<SpriteRenderer>();
            m_particleSystem ??= GetComponentInChildren<ParticleSystem>();
            
            m_activeProfile = anomalyProfile;
            m_spriteRenderer.color = m_activeProfile.color;
            
            var main = m_particleSystem.main;
            main.startColor = new ParticleSystem.MinMaxGradient(m_activeProfile.color);
        }

        private void CheckForCollisions()
        {
            var actors = GameController.Actors;

            foreach (var actor in actors)
            {
                if (CollisionChecks.CircleToCircle(
                        actor.transform.position,
                        actor.collisionRadius,
                        transform.position,
                        Radius) == false)
                {
                    //If we're no longer colliding, but were tracking this actor
                    if (m_overlappedActors.Contains(actor))
                    {
                        m_overlappedActors.Remove(actor);
                        RemoveAffectors(actor);
                    }
                    
                    continue;
                }
                
                //If we were already tracking the actor, don't add it again
                if(m_overlappedActors.Contains(actor))
                    continue;
                
                m_overlappedActors.Add(actor);
                ApplyAffectors(actor);
            }
        }

        //Overlap
        //============================================================================================================//

        private void ApplyAffectors(ActorBase actorBase)
        {
            foreach (var affector in m_activeProfile.affectors)
            {
                affector.Apply(actorBase);
            }
        }

        private void RemoveAffectors(ActorBase actorBase)
        {
            foreach (var affector in m_activeProfile.affectors)
            {
                affector.Remove(actorBase);
            }
        }

        //Unity Editor Functions
        //============================================================================================================//

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Draw.Circle(transform.position, Color.yellow, Radius);
        }

#endif
    }
}
