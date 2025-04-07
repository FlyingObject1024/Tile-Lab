using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    // �Z���T�C�Y��720px(480px)�ɂ���
    private int gridSize = 480;
    // 3.45f���قڃs�b�^�������A���m�Ȑ��l�ł͂Ȃ�
    private float cellSize = 3.45f;

    public Dictionary<Vector2Int, TileManager> grid_dic = new Dictionary<Vector2Int, TileManager>();

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
            Debug.LogWarning("�J������Orthographic�ł͂���܂���");
        }
    }

    public bool IsOccupied(Vector2Int pos)
    {
        TileManager tile;
        return grid_dic.TryGetValue(pos, out tile);
    }

    public void searchGateAt(Vector2Int pos)
    {

    }

    public void fireInput()
    {

    }

    public void SetTile(Vector2Int pos, TileManager tile)
    {
        grid_dic.Add(pos, tile);
        if (tile.tileType == TileType.Input)
        {
            fireInput();
        }
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
