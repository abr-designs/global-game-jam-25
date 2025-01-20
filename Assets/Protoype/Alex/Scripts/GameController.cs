using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Protoype.Alex
{
    public class GameController : HiddenSingleton<GameController>
    {
        public static IReadOnlyList<ActorBase> Actors => s_actors;
        private static List<ActorBase> s_actors;

        [SerializeField]
        private Anomaly anomaly;

        [SerializeField]
        private AnomalyProfile[] anomalyProfiles;

        private void Start()
        {
            anomaly.Setup(anomalyProfiles.PickRandomElement());
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
