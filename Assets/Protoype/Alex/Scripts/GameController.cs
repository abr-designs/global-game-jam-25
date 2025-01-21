using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Protoype.Alex
{
    public class GameController : HiddenSingleton<GameController>
    {
        public static IReadOnlyList<ActorBase> Actors => s_actors;
        private static List<ActorBase> s_actors;

        [SerializeField]
        private Anomaly anomalyPrefab;
        [SerializeField]
        private AnomalyProfile[] anomalyProfiles;

        private Rect m_screenBounds;
        
        [SerializeField]
        private Vector2 delayRange;
        
        private void Start()
        {
            var camera = Camera.main;
            m_screenBounds = new Rect
            {
                min = camera.ViewportToWorldPoint(Vector2.zero),
                max = camera.ViewportToWorldPoint(Vector2.one),
            };
            
            StartCoroutine(AnomalyCoroutine());
        }

        //Anomaly Generation
        //============================================================================================================//

        private IEnumerator AnomalyCoroutine()
        {
            yield return new WaitForSeconds(Random.Range(delayRange.x, delayRange.y));
            
            while (true)
            {
                //Pick Random Screen pos
                var worldPos = new Vector3(
                    Random.Range(m_screenBounds.xMin, m_screenBounds.xMax),
                    Random.Range(m_screenBounds.yMin, m_screenBounds.yMax),
                    0f);
                
                //Spawn Anomaly
                var anomaly = Instantiate(anomalyPrefab, worldPos, Quaternion.identity);
                
                //Pick & assign profile
                var profile = anomalyProfiles.PickRandomElement();
                anomaly.Setup(profile);
                //Wait for it to finish
                yield return new WaitForSeconds(profile.activeTime);
                
                //Destroy the GameObject
                Destroy(anomaly.gameObject);

                //Wait Random Time
                yield return new WaitForSeconds(Random.Range(delayRange.x, delayRange.y));
            }
            
        }

        //Actor Registration
        //============================================================================================================//
        
        public static void RegisterActor(ActorBase actorBase)
        {
            s_actors ??= new List<ActorBase>();
            
            if (s_actors.Contains(actorBase))
                return;

            s_actors.Add(actorBase);
        }

        public static void DeRegisterActor(ActorBase actorBase)
        {
            if (s_actors == null)
                return;
            
            if (s_actors.Contains(actorBase) == false)
                return;

            s_actors.Remove(actorBase);
        }
        //============================================================================================================//
    }
}
