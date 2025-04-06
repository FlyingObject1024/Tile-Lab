using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public int width = 10;
    public int height = 10;
    public float cellSize = 1.0f;

    public TileManager[,] grid;

    void Awake()
    {
        Instance = this;
        grid = new TileManager[width, height];
    }

    public bool IsOccupied(Vector2Int pos)
    {
        return grid[pos.x, pos.y] != null;
    }

    public void SetTile(Vector2Int pos, TileManager tile)
    {
        grid[pos.x, pos.y] = tile;
    }

    public void RemoveTile(Vector2Int pos)
    {
        grid[pos.x, pos.y] = null;
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
