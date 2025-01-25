using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "Scriptable Objects/AudioSettings")]
    public class AudioSettings : ScriptableObject
    {
        public float MasterVolume = 0.75f;
        public float MusicVolume = 1f;
        public float SFXVolume = 1f;
    }

}