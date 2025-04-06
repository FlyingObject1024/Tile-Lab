using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    // セルサイズを720px(480px+240px)にする
    public float cellSize = 2.0f;

    public Dictionary<Vector2Int, TileManager> grid_dic = new Dictionary<Vector2Int, TileManager>();

    void Awake()
    {
        Instance = this;
    }

    public bool IsOccupied(Vector2Int pos)
    {
        TileManager tile;
        return grid_dic.TryGetValue(pos, out tile);
    }

    public void SetTile(Vector2Int pos, TileManager tile)
    {
        grid_dic.Add(pos, tile);
    }

    public void RemoveTile(Vector2Int pos)
    {
        grid_dic.Remove(pos);
    }

    public Vector3 GridToWorld(Vector2Int pos)
    {
        return new Vector3(pos.x * cellSize, pos.y * cellSize, 0);
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPos.x / cellSize),
            Mathf.RoundToInt(worldPos.y / cellSize)
        );
    }
}
