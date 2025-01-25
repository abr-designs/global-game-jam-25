using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class SettingsUI : BaseUIWindow
    {
        [SerializeField, Header("Volume Settings")]
        private VolumeSettings masterVolume;
        [SerializeField]
        private VolumeSettings musicVolume;
        [SerializeField]
        private VolumeSettings sfxVolume;

        [SerializeField] Audio.AudioSettings audioSettings;

        private float MasterVolume => audioSettings.MasterVolume;
        private float MusicVolume => audioSettings.MusicVolume;
        private float SFXVolume => audioSettings.SFXVolume;

        [SerializeField] private AudioMixerGroup masterAudioMixer;


        //Unity Functions
        //============================================================================================================//

        private void OnEnable()
        {
            masterVolume.Init(OnMasterVolumeChanged, MasterVolume);
            musicVolume.Init(OnMusicVolumeChanged, MusicVolume);
            sfxVolume.Init(OnSFXVolumeChanged, SFXVolume);
        }

        private void OnDisable()
        {
            masterVolume.DeInit();
            musicVolume.DeInit();
            sfxVolume.DeInit();
        }

        //Set Volume Functions
        //============================================================================================================//

        private void OnMasterVolumeChanged(float value)
        {
            audioSettings.MasterVolume = value;
            var v = Mathf.Log10(value) * 20;
            masterAudioMixer.audioMixer.SetFloat("MasterVolume", v);
        }
        private void OnMusicVolumeChanged(float value)
        {
            audioSettings.MusicVolume = value;
            Audio.Music.MusicController.Instance.SetVolume(value);
        }
        private void OnSFXVolumeChanged(float value)
        {
            audioSettings.SFXVolume = value;
            Audio.SFXManager.Instance.SetVolume(value);
        }

        //Volume Settings Class
        //============================================================================================================//

        #region Volume Settings Class

        [Serializable]
        private class VolumeSettings
        {
            //If you want the Volume settings to be displayed in a reorderable list, you may want to enable the name
            //[SerializeField]
            //private string name;
            [SerializeField] private Slider slider;
            [SerializeField] private TMP_Text text;

            [SerializeField, Range(0f, 1f)] private float startingValue = 1f;
            private UnityAction<float> _callback;

            private void FormatText(float value)
            {
                text.text = $"{value * 100:0}%";
            }

            public void Init(UnityAction<float> callback, float overrideValue = -1)
            {
                Assert.IsNotNull(slider);

                slider.wholeNumbers = false;
                slider.minValue = 0f;
                slider.maxValue = 1f;
                _callback = callback;
                slider.onValueChanged.AddListener(_callback);

                if (text != null)
                    slider.onValueChanged.AddListener(FormatText);

                var newValue = overrideValue >= 0f ? overrideValue : startingValue;

                slider.SetValueWithoutNotify(newValue);
                FormatText(newValue);

                // Callback on init just to sync values
                callback(newValue);

            }

            public void DeInit()
            {
                slider.onValueChanged.RemoveListener(_callback);
                _callback = null;
                if (text != null)
                    slider.onValueChanged.RemoveListener(FormatText);
            }
        }

        #endregion //Volume Settings Class

        //============================================================================================================//

    }
}