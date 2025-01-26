using System;
using Audio.Music;
using GameInput;
using Levels;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGJ.BubbleFall
{
    public class GameManager : MonoBehaviour
    {
        internal static GameManager Instance;

        public enum GAME_STATE
        {
            LOAD,
            PLAY,
            PAUSE,
            END,
        }
        private GAME_STATE _gameState = GAME_STATE.LOAD;

        // Reference to player controller
        [SerializeField]
        private PlayerMovementV2 playerController;

        private CinemachineCamera _vCamera;

        private void OnEnable()
        {
            GoalZone.GoalHit += OnLevelCompleted;
            PlayerHealth.OnPlayerDeath += OnPlayerDeath;
        }
        private void OnDisable()
        {
            GoalZone.GoalHit -= OnLevelCompleted;
            PlayerHealth.OnPlayerDeath -= OnPlayerDeath;
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _vCamera = FindAnyObjectByType<CinemachineCamera>();
        }

        // Update is called once per frame
        void Update()
        {
            switch (_gameState)
            {
                case GAME_STATE.LOAD:

                    MusicController.Instance.PlayMusic(MUSIC.GAME);

                    // Load first level
                    LevelLoader.LoadFirstLevel();
                    InitPlayer(true);

                    _gameState = GAME_STATE.PLAY;

                    break;
                case GAME_STATE.PLAY:
                    break;
            }

        }

        private void OnLevelCompleted()
        {
            // Load next level?
            if (LevelLoader.LoadNextLevel())
            {
                InitPlayer(true);
            }
            else
            {
                // GAME FULLY DONE
                SceneManager.LoadScene(2);
            }

        }

        // 
        private void OnPlayerDeath()
        {
            // Play any death cinematic etc
            Debug.Log("Respawning player!");
            InitPlayer();
        }

        // Reset player position / health / ammo etc
        // Spawn at last spawn location (unless it's first time in level)
        private void InitPlayer(bool isStartSpawn = false)
        {
            GameInputDelegator.SetInputLock(true);
            if (isStartSpawn)
            {
                var currentLevel = (PlatformLevel)LevelLoader.CurrentLevelDataDefinition;
                var spawn = currentLevel.PlayerSpawnLocation;
                playerController.LastSpawnLocation = spawn;
            }

            playerController.transform.position = playerController.LastSpawnLocation.transform.position;
            playerController.GetComponent<PlayerHealth>().ResetHealth();
            playerController.GetComponent<CaptiveGatherController>().ResetCaptives();
            playerController.GetComponent<BubbleThrower>().Reset();
            playerController.gameObject.SetActive(true);
            GameInputDelegator.SetInputLock(false);

            // Move camera
            _vCamera.ForceCameraPosition(transform.position, Quaternion.identity);

        }
    }

}