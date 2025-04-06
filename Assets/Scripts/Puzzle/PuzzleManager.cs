using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public GameObject stage;

    public GameObject inputTilePrefab;
    public GameObject gateTilePrefab;
    public GameObject goalTilePrefab;

    string puzzle_name = "";

    int input_total_number = -1;
    int gate_total_number = -1;
    int goal_total_number = -1;

    private List<GameObject> tiles = new List<GameObject>();

    int tile_index = -1;

    void loadPrefabs()
    {
        // Resources フォルダからプレハブをロード
        inputTilePrefab = Resources.Load<GameObject>("Prefabs/InputTile");
        gateTilePrefab  = Resources.Load<GameObject>("Prefabs/GateTile");
        goalTilePrefab  = Resources.Load<GameObject>("Prefabs/GoalTile");

        if (inputTilePrefab == null || gateTilePrefab == null || goalTilePrefab == null)
        {
            Debug.LogError("いずれかのTileプレハブがResources/Prefabsに存在しません！");
        }
    }

    // Start Gatter and Setter Area

    public void setPuzzleName(GameObject s, string name)
    {
        stage = s;
        tile_index = -1;
        puzzle_name = name;
        this.name = "puzzle<" + name + ">";
        this.transform.SetParent(stage.transform);
        loadPrefabs();
    }

    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }

    public string getPuzzleName()
    {
        return puzzle_name;
    }

    public void setInputTotalNumber(int num)
    {
        input_total_number = num;
    }

    public int getInputTotalNumber()
    {
        return input_total_number;
    }

    public void setGateTotalNumber(int num)
    {
        gate_total_number = num;
    }

    public int getGateTotalNumber()
    {
        return gate_total_number;
    }

    public void setGoalTotalNumber(int num)
    {
        goal_total_number = num;
    }

    public int getGoalTotalNumber()
    {
        return goal_total_number;
    }

    // End Gatter and Setter Area

    public void createInputTile(int size)
    {
        if (inputTilePrefab != null)
        {
            tile_index++;

            GameObject tile = Instantiate(inputTilePrefab);
            tile.name = "input<" + puzzle_name + "-" + tile_index + ">";
            tile.transform.SetParent(this.transform);

            // TileManager設定
            TileManager tileManager = tile.GetComponent<TileManager>();
            if (tileManager != null)
            {
                tileManager.tileType = TileType.Input;
            }
            tiles.Add(tile);
        }
    }

    public void createGateTile(string name, string type)
    {
        tile_index++;

        GameObject tile = Instantiate(gateTilePrefab);
        tile.name = type + "-gate<" + puzzle_name + "-" + tile_index + ">";
        tile.transform.SetParent(this.transform);

        // TileManager設定
        TileManager tileManager = tile.GetComponent<TileManager>();
        if (tileManager != null)
        {
            tileManager.tileType = TileType.Gate;
            if (type == "uni")
            {
                tileManager.gateType = GateType.Uni;
            }
            else if(type == "bi")
            {
                tileManager.gateType = GateType.Bi;
            }
            tileManager.setGateName(name);
            tileManager.init();
        }

        tiles.Add(tile);
    }

    public void createGoalTile(int size)
    {
        if (goalTilePrefab != null)
        {
            tile_index++;

            GameObject tile = Instantiate(goalTilePrefab);
            tile.name = "goal<" + puzzle_name + "-" + tile_index + ">";
            tile.transform.SetParent(this.transform);

            TileManager tileManager = tile.GetComponent<TileManager>();
            if (tileManager != null)
            {
                tileManager.tileType = TileType.Goal;
            }

            tiles.Add(tile);
        }
    }

    public void setMatrix(int size, List<List<int>> mat)
    {
        GameObject tile = tiles[tile_index];
        TileManager tileManager = tile.GetComponent<TileManager>();
        if (tileManager != null)
        {
            Debug.Log(this.name + " "+ tileManager.name + " " + tiles.Count);
            tileManager.setMatrix(size, mat);
            tileManager.init();
        }
    }
}
