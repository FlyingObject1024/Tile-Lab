using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    // セルサイズを720px(480px)にする
    private int gridSize = 480;
    // 3.45fがほぼピッタリだが、正確な数値ではない
    private float cellSize = 3.45f;

    public Dictionary<Vector2Int, TileManager> grid_dict = new Dictionary<Vector2Int, TileManager>();
    public Dictionary<Vector2Int, TileManager> input_dict = new Dictionary<Vector2Int, TileManager>();

    public List<Vector2Int> direction_list = new List<Vector2Int>
        {
            new Vector2Int(1, 0),   // 右
            new Vector2Int(0, 1),   // 上
            new Vector2Int(-1, 0),   // 左
            new Vector2Int(0, -1)  // 下
        };

    string[] arrow = { "→", "↑", "←", "↓" };

    void Awake()
    {
        Instance = this;
        Camera cam = Camera.main;
        if (cam.orthographic)
        {
            float pixelsPerUnit = Screen.height / (cam.orthographicSize * 2);
            //cellSize = gridSize / pixelsPerUnit;
            Debug.Log("cellsize: " + cellSize);
        }
        else
        {
            Debug.LogWarning("カメラがOrthographicではありません");
        }
    }

    public bool IsOccupied(Vector2Int pos)
    {
        TileManager tile;
        return grid_dict.TryGetValue(pos, out tile);
    }

    public bool searchAcceptedGateAt(Vector2Int search_pos, int direction, List<List<int>> input_matrix)
    {
        TileManager tile;
        if (grid_dict.TryGetValue(search_pos, out tile))
        {
            Debug.Log("Hit at: " + search_pos + " " + tile.PrintMatrix(input_matrix));
            return tile.requestInput(direction, input_matrix);
        }
        return false;
    }

    // 配置されたInputタイルを起点に4近傍にあるGateタイルを探す
    public void fireInputs()
    {
        Debug.Log("Search inputs");
        foreach (var input in input_dict)
        {
            Vector2Int pos = input.Key;
            TileManager input_tile = input.Value;
            Debug.Log("Search gate from: " + input_tile.name + input_tile.gridPosition);
            for (int i = 0; i < direction_list.Count; i++)
            {
                Vector2Int search_pos = pos + direction_list[i];
                //Debug.Log(input_tile.PrintMatrix(input_tile.matrix));
                if(searchAcceptedGateAt(search_pos, i, input_tile.matrix))
                {
                    Debug.Log("fire: " + input_tile.name + arrow[i]);
                }
            }
        }
    }

    public void SetTile(Vector2Int pos, TileManager tile)
    {
        grid_dict.Add(pos, tile);
        if (tile.tileType == TileType.Input)
        {
            Debug.Log("SetTile: " + tile.name + " " + tile.PrintMatrix(tile.matrix));
            input_dict.Add(pos, tile);
        }
        fireInputs();
    }

    public void RemoveTile(Vector2Int pos)
    {
        grid_dict.Remove(pos);
        input_dict.Remove(pos);
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
