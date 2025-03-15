using System;
using UnityEngine;

namespace GenericServiceLocator.Samples.Runtime.AudioModule
{
    /// <summary>
    /// Concrete implementation of IAudioService.
    /// </summary>
    public class AudioService : MonoBehaviour, IAudioService
    {
        [SerializeField] private AudioSource m_audioSource;
        [SerializeField] private AudioClip m_audioClip;

        private void Awake()
        {
            m_audioSource ??= GetComponent<AudioSource>();
            m_audioSource.clip = m_audioClip;
        }

        public void PlaySound(string soundName)
        {
            m_audioSource.Play();
            Debug.Log(message: "[AudioService]: Playing sound " + soundName);
        }
    }
}