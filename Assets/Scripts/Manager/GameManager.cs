using System;
using Audio.Music;
using Levels;
using UnityEngine;

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

        private void OnEnable()
        {
            GoalZone.GoalHit += OnLevelCompleted;
        }
        private void OnDisable()
        {
            GoalZone.GoalHit -= OnLevelCompleted;
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
                    InitPlayer();

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
                InitPlayer();
            }
            else
            {
                // GAME FULLY DONE
                // TODO -- End credits scene
            }

        }

        // Reset player position / health / ammo etc
        private void InitPlayer()
        {

            var currentLevel = (PlatformLevel)LevelLoader.CurrentLevelDataDefinition;
            var spawn = currentLevel.PlayerSpawnLocation;
            playerController.transform.position = spawn.transform.position;

        }
    }

}