using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public enum TileType { Input, Gate, Output, Goal }
public enum GateType { None, Uni, Bi }
public enum SpinDirection { Clockwise, CounterClockwise }

public class TileManager : MonoBehaviour
{
    public static string tile_0_path = "Images/tile_0";
    public string tile_1_path = "Images/tile_1";

    public string unigate_path = "Images/unigate";
    public string bigate_path = "Images/bigate";

    public string correct_icon_path = "Images/correct";
    public string wrong_icon_path = "Images/wrong";

    public GameObject outputTilePrefab;

    public TileType tileType;
    public GateType gateType;
    public string gatename = "None";

    public Vector2Int gridPosition;
    public bool isPlaced = false;
    public bool isDraggable = false;

    int matrix_size = 0;
    public List<List<int>> matrix = new List<List<int>>();

    int tile_direction = 0;

    SpriteRenderer renderer;
    int sprite_height = 0;
    int sprite_width = 0;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public string PrintMatrix(List<List<int>> mat)
    {
        string s = mat.Count + ":";
        
        for (int y = 0; y < mat.Count; y++)
        {
            for (int x = 0; x < mat[y].Count; x++)
            {
                s = s + mat[y][x];
            }
            s = s + "\n";
        }
        return s;
    }

    public void setMatrix(int size, List<List<int>> mat)
    {
        matrix_size = size;
        matrix = mat;
    }

    public List<List<int>> getMatrix()
    {
        return matrix;
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

        // 3x3版に未対応
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

                if (x == 0)
                {
                    sprite_name = sprite_name + value;
                }
                else
                {
                    sprite_name = sprite_name + "-" + value;
                }
            }
            if (y < matrix.Count - 1)
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
        else if (gateType == GateType.Bi)
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
        setMatrixSprite();
    }

    public void init()
    {
        outputTilePrefab = Resources.Load<GameObject>("Prefabs/OutputTile");

        if (outputTilePrefab == null)
        {
            Debug.LogError("いずれかのTileプレハブがResources/Prefabsに存在しません！");
        }

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

        if (tileType == TileType.Input || tileType == TileType.Gate)
        {
            isDraggable = true;
            BoxCollider2D box_collider_2d = GetComponent<BoxCollider2D>();
            box_collider_2d.size = new Vector2(2f, 2f);
        }
    }

    // 入力する行列を受け付ける
    public bool requestInput(int direction, List<List<int>> input_matrix)
    {
        if (tileType == TileType.Gate)
        {
            if (gateType != GateType.None && direction == tile_direction)
            {
                // 通常方向
                Vector2Int search_pos = gridPosition + GridManager.Instance.direction_list[tile_direction];
                List<List<int>> output_matrix = calcOutput(input_matrix);
                if (GridManager.Instance.searchAcceptedGateAt(search_pos, direction, output_matrix))
                {
                    Debug.Log("hop " + gatename);
                    return true;
                }
                else
                {
                    generateOutput(direction, output_matrix);
                    return true;
                }
            }
            if (gateType == GateType.Bi && (direction == ((tile_direction + 3) % 4)))
            {
                // 2つめの方向
                Vector2Int search_pos = gridPosition + GridManager.Instance.direction_list[direction];
                List<List<int>> output_matrix = calcOutput(input_matrix);
                if (GridManager.Instance.searchAcceptedGateAt(search_pos, direction, output_matrix))
                {
                    Debug.Log("hop " + gatename);
                    return true;
                }
                else
                {
                    generateOutput(direction, output_matrix);
                    return true;
                }
            }
        }
        return false;
    }

    // outputを生成する
    public bool generateOutput(int direction, List<List<int>> output_matrix)
    {
        GameObject new_output_tile = Instantiate(outputTilePrefab);
        new_output_tile.name = "output: " + this.name;
        new_output_tile.transform.SetParent(this.transform);

        TileManager tileManager = new_output_tile.GetComponent<TileManager>();
        if (tileManager != null)
        {
            tileManager.tileType = TileType.Output;
            tileManager.setMatrix(output_matrix.Count, output_matrix);
            tileManager.setMatrixSprite();

            Vector2Int newPosition = gridPosition + GridManager.Instance.direction_list[direction];
            
            if(GridManager.Instance.SetOutputTile(newPosition, new_output_tile))
            {
                Debug.Log("Generate: \n" + PrintMatrix(output_matrix));
                new_output_tile.transform.position = GridManager.Instance.GridToWorld(newPosition);
                return true;
            }
            else
            {
                Destroy(new_output_tile);
            }
        }

        return false;
    }

    // 各種Gate処理
    List<List<int>> inversion(List<List<int>> input_matrix)
    {
        //0->1, 1->0に変換
        int n = input_matrix.Count;
        var output_matrix = new List<List<int>>();

        for (int i = 0; i < n; i++)
        {
            var row = new List<int>();
            for (int j = 0; j < n; j++)
            {
                row.Add(input_matrix[i][j] == 0 ? 1 : 0);
            }
            output_matrix.Add(row);
        }

        return output_matrix;
    }

    List<List<int>> xFlip(List<List<int>> input_matrix)
    {
        //x軸で反転
        int n = input_matrix.Count;
        var output_matrix = new List<List<int>>();

        for (int i = n - 1; i >= 0; i--)
        {
            output_matrix.Add(new List<int>(input_matrix[i]));
        }

        return output_matrix;
    }

    List<List<int>> yFlip(List<List<int>> input_matrix)
    {
        //y軸で反転
        int n = input_matrix.Count;
        var output_matrix = new List<List<int>>();

        for (int i = 0; i < n; i++)
        {
            var flippedRow = new List<int>();
            for (int j = n - 1; j >= 0; j--)
            {
                flippedRow.Add(input_matrix[i][j]);
            }
            output_matrix.Add(flippedRow);
        }

        return output_matrix;
    }

    List<List<int>> diagonalFlip(List<List<int>> input_matrix)
    {
        //対角線で反転
        int n = input_matrix.Count;
        var output_matrix = new List<List<int>>();

        for (int i = 0; i < n; i++)
        {
            var row = new List<int>();
            for (int j = 0; j < n; j++)
            {
                row.Add(input_matrix[j][i]);
            }
            output_matrix.Add(row);
        }

        return output_matrix;
    }

    List<List<int>> reverseDiagonalFlip(List<List<int>> input_matrix)
    {
        // ↙ 逆対角線で反転
        int n = input_matrix.Count;
        var output_matrix = new List<List<int>>(Enumerable.Range(0, n).Select(_ => new List<int>(new int[n])));

        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                output_matrix[n - 1 - j][n - 1 - i] = input_matrix[i][j];

        return output_matrix;
    }

    List<List<int>> rotate90(List<List<int>> input_matrix)
    {
        // 反時計回り90度回転
        int n = input_matrix.Count;
        var output_matrix = new List<List<int>>();

        for (int i = 0; i < n; i++)
        {
            var row = new List<int>();
            for (int j = 0; j < n; j++)
            {
                row.Add(input_matrix[j][n - 1 - i]);
            }
            output_matrix.Add(row);
        }

        return output_matrix;
    }

    List<List<int>> rotate180(List<List<int>> input_matrix)
    {
        // 反時計回り180度回転
        return rotate90(rotate90(input_matrix));
    }

    List<List<int>> rotate270(List<List<int>> input_matrix)
    {
        // 反時計回り270度回転
        return rotate90(rotate180(input_matrix));
    }

    List<List<int>> firstRowSlide(List<List<int>> input_matrix)
    {
        // 最初の行を最後へ移動
        var output_matrix = new List<List<int>>(input_matrix.Skip(1));
        output_matrix.Add(new List<int>(input_matrix[0]));
        return output_matrix;
    }

    List<List<int>> firstColSlide(List<List<int>> input_matrix)
    {
        // 最初の列を最後へ移動
        var output_matrix = new List<List<int>>();
        foreach (var row in input_matrix)
        {
            var newRow = row.Skip(1).ToList();
            newRow.Add(row[0]);
            output_matrix.Add(newRow);
        }
        return output_matrix;
    }

    List<List<int>> lastRowSlide(List<List<int>> input_matrix)
    {
        // 最後の行を最初へ移動
        var output_matrix = new List<List<int>>();
        output_matrix.Add(new List<int>(input_matrix.Last()));
        output_matrix.AddRange(input_matrix.Take(input_matrix.Count - 1));
        return output_matrix;
    }

    List<List<int>> lastColSlide(List<List<int>> input_matrix)
    {
        // 最後の列を最初へ移動
        var output_matrix = new List<List<int>>();
        foreach (var row in input_matrix)
        {
            var newRow = new List<int> { row.Last() };
            newRow.AddRange(row.Take(row.Count - 1));
            output_matrix.Add(newRow);
        }
        return output_matrix;
    }

    // Gate分岐
    List<List<int>> calcOutput(List<List<int>> input_matrix)
    {
        if (gatename == "inversion")
        {
            return inversion(input_matrix);
        }
        else if (gatename == "xFlip")
        {
            return xFlip(input_matrix);
        }
        else if (gatename == "yFlip")
        {
            return yFlip(input_matrix);
        }
        else if (gatename == "diagonalFlip")
        {
            return diagonalFlip(input_matrix);
        }
        else if (gatename == "reverseDiagonalFlip")
        {
            return reverseDiagonalFlip(input_matrix);
        }
        else if (gatename == "rotate90")
        {
            return rotate90(input_matrix);
        }
        else if (gatename == "rotate180")
        {
            return rotate180(input_matrix);
        }
        else if (gatename == "rotate270")
        {
            return rotate270(input_matrix);
        }
        else if (gatename == "firstRowSlide")
        {
            return firstRowSlide(input_matrix);
        }
        else if (gatename == "firstColSlide")
        {
            return firstColSlide(input_matrix);
        }
        else if (gatename == "lastRowSlide")
        {
            return lastRowSlide(input_matrix);
        }
        else if (gatename == "lastColSlide")
        {
            return lastColSlide(input_matrix);
        }
        else
        {
            return input_matrix;
        }
    }

    // tile_direction
    public int getTileRotate()
    {
        return tile_direction;
    }

    // Gate回転処理
    public void spinTile(SpinDirection spin)
    {
        if (spin == SpinDirection.Clockwise)
        {
            tile_direction++;
            tile_direction = tile_direction % 4;
            this.transform.Rotate(0f, 0f, 90f);
        }
        else if (spin == SpinDirection.CounterClockwise)
        {
            tile_direction--;
            if (tile_direction < 0) tile_direction = 3;
            this.transform.Rotate(0f, 0f, -90f);
        }
    }
}
