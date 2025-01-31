using Unity.VisualScripting;
using UnityEngine;

// Represents a preset part of a level that will be combined to create a random layout

public class LevelTile : MonoBehaviour
{

    public static float TileSize = 20f;

    // whether the tile changes elevation
    public enum TILE_TYPE
    {
        START = 0,
        GOAL,
        PLAIN,
        ASCENDING,
        DESCENDING
    }

    public TILE_TYPE TileType = TILE_TYPE.PLAIN;

    // Draw the bounds
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Vector3.one * TileSize);
    }

}
