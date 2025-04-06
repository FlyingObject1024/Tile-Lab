using System.Collections.Generic;
using UnityEngine;

public enum TileType { Input, Gate, Output, Goal }
public enum GateType { None, Uni, Bi }
public enum SpinDirection { Clockwise, CounterClockwise }

public class TileManager : MonoBehaviour
{
    public string tile_0_path = "Images/tile_0";
    public string tile_1_path = "Images/tile_1";

    public string unigate_path = "Images/unigate";
    public string bigate_path = "Images/bigate";

    public string correct_icon_path = "Images/correct";
    public string wrong_icon_path = "Images/wrong";

    public TileType tileType;
    public GateType gateType;
    public string gatename = "None";

    public Vector2Int gridPosition;
    public bool isPlaced = false;
    public bool isDraggable = false;

    public float placed_x = 0f;
    public float placed_y = 0f;

    int matrix_size = 0;
    private List<List<int>> matrix = new List<List<int>>();

    int tile_rotation = 0;

    SpriteRenderer renderer;
    int sprite_height = 0;
    int sprite_width = 0;

    public void setMatrix(int size, List<List<int>> mat)
    {
        matrix_size = size;
        matrix = mat;
    }

    public void setGateName(string name)
    {
        gatename = name;
    }

    public void PlaceAt(Vector2Int gridPos)
    {
        gridPosition = gridPos;
        transform.position = GridManager.Instance.GridToWorld(gridPos);
        GridManager.Instance.SetTile(gridPos, this);
    }

    public void RemoveFromGrid()
    {
        GridManager.Instance.RemoveTile(gridPosition);
    }

    void setMatrixSprite()
    {
        Debug.Log("setMatrixSprite");
        // 小さいタイル画像を読み込み
        Sprite tile0_sprite = Resources.Load<Sprite>(tile_0_path);
        Sprite tile1_sprite = Resources.Load<Sprite>(tile_1_path);

        if (tile0_sprite == null || tile1_sprite == null)
        {
            Debug.LogError("Tile sprites not found.");
            return;
        }
        
        Texture2D tile0_tex = tile0_sprite.texture;
        Texture2D tile1_tex = tile1_sprite.texture;

        int tile_width = tile0_tex.width;
        int tile_height = tile0_tex.height;

        // 2x2枚を貼り付けるので、全体サイズは2倍
        sprite_width = tile_width * 2;
        sprite_height = tile_height * 2;

        Texture2D newTexture = new Texture2D(sprite_width, sprite_height, TextureFormat.RGBA32, false);

        Debug.Log("generateSprite");

        string sprite_name = "";
        // 貼り付け
        for (int y = 0; y < matrix.Count; y++)
        {
            for (int x = 0; x < matrix[y].Count; x++)
            {
                int value = matrix[y][x];
                Texture2D sourceTex = value == 0 ? tile0_tex : tile1_tex;

                // 貼り付け位置
                int px = x * tile_width;
                int py = ((matrix_size - 1) - y) * tile_height;

                // sourceTexからピクセル情報を取り出して、newTextureに貼る
                Color[] pixels = sourceTex.GetPixels();
                newTexture.SetPixels(px, py, tile_width, tile_height, pixels);

                if(x == 0)
                {
                    sprite_name = sprite_name + value;
                }
                else
                {
                    sprite_name = sprite_name + "-" + value;
                }
            }
            if(y < matrix.Count - 1)
            {
                sprite_name = sprite_name + "_";
            }
        }

        newTexture.Apply(); // 変更を確定

        Debug.Log("attachSprite: " + sprite_name);
        // 新しいスプライトを作成して貼り付け
        Rect rect = new Rect(0, 0, sprite_width, sprite_height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Sprite newSprite = Sprite.Create(newTexture, rect, pivot);

        newSprite.name = sprite_name;
        renderer.sprite = newSprite;
        renderer.sortingOrder = 1;
    }

    void setGateSprite()
    {
        Sprite gate_sprite = null;

        if (gateType == GateType.Uni)
        {
            gate_sprite = Resources.Load<Sprite>(unigate_path);
        }
        else if(gateType == GateType.Bi)
        {
            gate_sprite = Resources.Load<Sprite>(bigate_path);
        }

        if (gate_sprite == null)
        {
            Debug.LogError("Gate sprites not found.");
            return;
        }

        renderer.sprite = gate_sprite;
        renderer.sortingOrder = 1;
    }

    void inputInit()
    {
        Debug.Log("input-mat");
        setMatrixSprite();
    }

    void gateInit()
    {
        setGateSprite();
    }

    void goalInit()
    {
        Debug.Log("goal-mat");
        setMatrixSprite();
    }

    void outputInit()
    {

    }

    public void init()
    {
        renderer = GetComponent<SpriteRenderer>();
        if (tileType == TileType.Input)
        {
            inputInit();
        }
        else if (tileType == TileType.Gate)
        {
            gateInit();
        }
        else if (tileType == TileType.Goal)
        {
            goalInit();
        }
        else if (tileType == TileType.Output)
        {
            //outputInit();
        }

        if(tileType == TileType.Input || tileType == TileType.Gate)
        {
            isDraggable = true;
            BoxCollider2D box_collider_2d = GetComponent<BoxCollider2D>();
            box_collider_2d.size = new Vector2(2f,2f);
        }
    }

    // Input起点
    public void startGenerateOutput()
    {

    }

    // Input・gate処理
    public bool generateOutput(int input_matrix_size, List<List<int>> input_matrix)
    {
        if(!(2 <= input_matrix_size && input_matrix_size <= 3))
        {
            return false;
        }
        // 隣のgateを見つける

        // gateのgenerateOutputを出力させる

        // gateの行先もgateなら、再帰で呼ぶ

        return false;
    }


    // Gate回転処理
    public void spinTile(SpinDirection spin)
    {
        if (spin == SpinDirection.Clockwise)
        {
            tile_rotation++;
            tile_rotation = tile_rotation % 4;
        }
        else if (spin == SpinDirection.CounterClockwise)
        {
            tile_rotation--;
            if (tile_rotation < 0) tile_rotation = 3;
        }
    }
}
