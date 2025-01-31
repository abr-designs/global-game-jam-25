using System.Collections.Generic;
using Levels;
using UnityEditor;
using UnityEngine;

namespace GGJ.BubbleFall
{

    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField]
        private string TilePrefabFolder = "Assets/Prefabs/Level/Tiles";

        private Dictionary<LevelTile.TILE_TYPE, List<LevelTile>> levelTilesDict;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void BuildTileList()
        {
            string[] folders = { TilePrefabFolder };
            string[] guids = AssetDatabase.FindAssets("t:prefab", folders);
            levelTilesDict = new Dictionary<LevelTile.TILE_TYPE, List<LevelTile>>();


            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var tile = AssetDatabase.LoadAssetAtPath<LevelTile>(path);

                if (!levelTilesDict.ContainsKey(tile.TileType))
                {
                    levelTilesDict.Add(tile.TileType, new List<LevelTile>());
                }

                levelTilesDict[tile.TileType].Add(tile);
            }
        }

        LevelTile GetRandomTile(LevelTile.TILE_TYPE type)
        {
            var list = levelTilesDict[type];
            var rndIdx = Random.Range(0, list.Count);
            return list[rndIdx];
        }

        LevelTile GetRandomMiddleTile()
        {
            var t = (LevelTile.TILE_TYPE)Random.Range(2, System.Enum.GetValues(typeof(LevelTile.TILE_TYPE)).Length);
            return GetRandomTile(t);
        }


        void CreateTile(LevelTile prefab, Vector2 position, Transform parent)
        {
            if (!prefab) return;
            var newTile = Instantiate<LevelTile>(prefab);
            newTile.transform.parent = parent;
            newTile.transform.position = new Vector3(position.x, position.y, 0);

        }

        [ContextMenu("Generate New Level")]
        void GenerateLevel()
        {
            // Load all tile prefabs
            BuildTileList();

            GameObject levelContainer = new GameObject("New Level");

            int currentElevation = 0;
            Vector2 currPos = Vector2.zero;
            LevelTile prevTile = null;

            // Pick start tile
            var startTile = GetRandomTile(LevelTile.TILE_TYPE.START);
            CreateTile(startTile, currPos, levelContainer.transform);
            currPos += Vector2.right * LevelTile.TileSize;

            // TODO -- pull these into parameters
            int levelSize = Random.Range(5, 10);
            for (int i = 0; i < levelSize; i++)
            {
                // Pick type of tile 
                var newTile = GetRandomMiddleTile();
                CreateTile(newTile, currPos, levelContainer.transform);

                // Adjust next tile position
                if (newTile.TileType == LevelTile.TILE_TYPE.ASCENDING)
                    currentElevation++;
                else if (newTile.TileType == LevelTile.TILE_TYPE.DESCENDING)
                    currentElevation--;
                currPos = new Vector2(currPos.x + LevelTile.TileSize, currentElevation * LevelTile.TileSize / 2f);
            }

            // Pick end tile
            var goalTile = GetRandomTile(LevelTile.TILE_TYPE.GOAL);
            CreateTile(goalTile, currPos, levelContainer.transform);

            var levelData = levelContainer.AddComponent<PlatformLevel>();
            levelData.PlayerSpawnLocation = startTile.transform.Find("SpawnPoint").gameObject;


        }
    }


}