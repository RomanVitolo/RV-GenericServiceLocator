using System;
using GenericServiceLocator.Runtime;
using UnityEngine;

namespace GenericServiceLocator.Samples.Runtime.AudioModule
{
      /// <summary>
      /// Installer responsible for registering the AudioService.
      /// </summary>
      public class AudioServiceInstaller : MonoBehaviour
      {
          public AudioService servicePrefab;

          private void Awake() => InstallService();
          private void InstallService()
          {
              if (ServiceLocator.TryGet<IAudioService>(service: out _)) return;
              AudioService audioInstance;

              if (servicePrefab != null)
              {
                  audioInstance = Instantiate(servicePrefab);
                  if (audioInstance != null) audioInstance.PlaySound("MainMenuSound");
              }
              else
              {
                  GameObject serviceGO = new GameObject("AudioService");
                  audioInstance = serviceGO.AddComponent<AudioService>();
                  audioInstance.PlaySound("MainMenuSound");
              }
                  
              DontDestroyOnLoad(audioInstance.gameObject);
                  
              ServiceLocator.Register<IAudioService>(audioInstance);
          }

          private void OnDestroy() => ServiceLocator.Unregister<IAudioService>();
      }
}
