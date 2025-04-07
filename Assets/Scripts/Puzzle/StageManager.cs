using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    GameObject main_field;
    UiManager ui_manager;
    List<GameObject> puzzles = new List<GameObject>();

    static int puzzle_total_num = 0;
    static int reading_puzzle_index = -1;
    static string reading_puzzle_name = "";

    public enum ReadModeState
    {
        Neutral,
        ReadInputs,
        ReadInputTiles,
        ReadGates,
        ReadGateTiles,
        ReadGoals,
        ReadGoalTiles,
    }

    static ReadModeState reading_mode = ReadModeState.Neutral;

    static int matrix_size = 0;
    static List<List<int>> reading_matrix = new List<List<int>>();

    static int readline_buffer = 0;

    static int forcus_puzzle_num = 0;

    // ファイル読み込み系関数
    void readPuzzleNumber(string line)
    {
        string[] parts = line.Split(",");
        puzzle_total_num = int.Parse(parts[1]);
    }

    int readInputs(string[] parts, int line_index)
    {
        if (reading_mode == ReadModeState.ReadInputs)
        {
            int tilesize;
            // タイルのサイズを渡す
            if (int.TryParse(parts[0], out tilesize))
            {
                PuzzleManager puzzle_manager = puzzles[reading_puzzle_index].GetComponent<PuzzleManager>();
                puzzle_manager.createInputTile(tilesize);
                readline_buffer = tilesize;
                matrix_size = tilesize;
                reading_mode = ReadModeState.ReadInputTiles;
            }
            else
            {
                Debug.LogError("Input tile size Numeric Error");
                return -100;
            }
        }
        else if (reading_mode == ReadModeState.ReadInputTiles)
        {
            readMatrixLine(parts, line_index);
            readline_buffer--;
            if (readline_buffer == 0)
            {
                Debug.Log("setmatrix from puzzle: " + reading_puzzle_index);

                PuzzleManager puzzle_manager = puzzles[reading_puzzle_index].GetComponent<PuzzleManager>();
                puzzle_manager.setMatrix(matrix_size, reading_matrix);
                //reading_matrix.Clear();
                reading_matrix = new List<List<int>>();
                reading_mode = ReadModeState.ReadInputs;
            }
        }

        return 0;
    }

    int readMatrixLine(string[] parts, int line_index)
    {
        List<int> row = new List<int>();
        for (int i = 0; i < parts.Length; i++)
        {
            row.Add(int.Parse(parts[i]));
        }
        reading_matrix.Add(row);

        return 0;
    }

    int readGates(string[] parts, int line_index)
    {
        if (reading_mode == ReadModeState.ReadGates)
        {
            PuzzleManager puzzle_manager = puzzles[reading_puzzle_index].GetComponent<PuzzleManager>();
            
            readline_buffer = puzzle_manager.getGateTotalNumber();
            reading_mode = ReadModeState.ReadGateTiles;
        }

        if (reading_mode == ReadModeState.ReadGateTiles)
        {
            if (parts.Length != 2)
            {
                Debug.LogError("Gate tile Error");
                return -101;
            }

            PuzzleManager puzzle_manager = puzzles[reading_puzzle_index].GetComponent<PuzzleManager>();
            puzzle_manager.createGateTile(parts[0], parts[1]);

            readline_buffer--;
            if (readline_buffer == 0)
            {
                reading_mode = ReadModeState.Neutral;
            }
        }
        return 0;
    }

    int readGoals(string[] parts, int line_index)
    {
        if (reading_mode == ReadModeState.ReadGoals)
        {
            int tilesize;
            // タイルのサイズを渡す
            if (int.TryParse(parts[0], out tilesize))
            {
                PuzzleManager puzzle_manager = puzzles[reading_puzzle_index].GetComponent<PuzzleManager>();
                puzzle_manager.createGoalTile(tilesize);
                readline_buffer = tilesize;
                matrix_size = tilesize;
                reading_mode = ReadModeState.ReadGoalTiles;
            }
            else
            {
                Debug.LogError("Goal tile size Numeric Error");
                return -102;
            }
        }
        else if (reading_mode == ReadModeState.ReadGoalTiles)
        {
            readMatrixLine(parts, line_index);
            readline_buffer--;
            if (readline_buffer == 0)
            {
                Debug.Log("setmatrix from puzzle: " + reading_puzzle_index);
                PuzzleManager puzzle_manager = puzzles[reading_puzzle_index].GetComponent<PuzzleManager>();
                puzzle_manager.setMatrix(matrix_size, reading_matrix);
                //reading_matrix.Clear();
                reading_matrix = new List<List<int>>();
                reading_mode = ReadModeState.ReadGoals;
            }
        }

        return 0;
    }

    int readEachTiles(string[] parts, int line_index)
    {
        if (reading_mode == ReadModeState.ReadInputs 
            || reading_mode == ReadModeState.ReadInputTiles)
        {
            return readInputs(parts, line_index);
        }
        else if (reading_mode == ReadModeState.ReadGates
            || reading_mode == ReadModeState.ReadGateTiles)
        {
            return readGates(parts, line_index);
        }
        else if (reading_mode == ReadModeState.ReadGoals
            || reading_mode == ReadModeState.ReadGoalTiles)
        {
            return readGoals(parts, line_index);
        }

        return 0;
    }

    int readPuzzleInfo(string line, int line_index)
    {
        string[] parts = line.Split(",");

        if      (parts[0] == "puzzle")
        {
            reading_mode = ReadModeState.Neutral;

            GameObject puzzlePrefab = Resources.Load<GameObject>("Prefabs/Puzzle");
            GameObject puzzleInstance = Instantiate(puzzlePrefab);
            PuzzleManager puzzle_manager = puzzleInstance.GetComponent<PuzzleManager>();

            reading_puzzle_index++;
            Debug.Log("reading_index: " + reading_puzzle_index + "/"+ puzzle_total_num);

            if (reading_puzzle_index >= puzzle_total_num)
            {
                Debug.LogError("Puzzle Total Over");
                return -2;
            }

            reading_puzzle_name = parts[1];
            if (puzzle_manager == null || reading_puzzle_index > puzzle_total_num)
            {
                return -1;
            }

            puzzle_manager.setPuzzleName(this.gameObject, reading_puzzle_name);

            // puzzle<1>以降は非アクティブにする(負荷軽減)
            if (reading_puzzle_index == 0) puzzle_manager.SetActive(true);
            else                           puzzle_manager.SetActive(false);

            puzzles.Add(puzzleInstance);
            Debug.Log("puzzles.Count: " + puzzles.Count);
        }
        else if (parts[0] == "inputs")
        {
            reading_mode = ReadModeState.ReadInputs;
            PuzzleManager puzzle_manager = puzzles[reading_puzzle_index].GetComponent<PuzzleManager>();
            puzzle_manager.setInputTotalNumber(int.Parse(parts[1]));
        }
        else if (parts[0] == "gates")
        {
            reading_mode = ReadModeState.ReadGates;
            PuzzleManager puzzle_manager = puzzles[reading_puzzle_index].GetComponent<PuzzleManager>();
            puzzle_manager.setGateTotalNumber(int.Parse(parts[1]));

        }
        else if (parts[0] == "goals")
        {
            reading_mode = ReadModeState.ReadGoals;
            PuzzleManager puzzle_manager = puzzles[reading_puzzle_index].GetComponent<PuzzleManager>();
            puzzle_manager.setGoalTotalNumber(int.Parse(parts[1]));
        }
        else
        {
            return readEachTiles(parts, line_index);
        }

        return 0;
    }

    public void readPuzzle(string stage_name)
    {
        string puzzle_file_path = "Assets/Data/" + stage_name + ".csv";
        FileStream data_file = new FileStream(puzzle_file_path, FileMode.Open, FileAccess.Read);
        StreamReader data_reader = new StreamReader(data_file);

        int line_index = 0;

        // ファイル冒頭の読み込み
        readPuzzleNumber(data_reader.ReadLine());
        line_index++;

        // 以降ファイルを1行毎に読み込み
        while (data_reader.Peek() != -1)
        {
            string line = data_reader.ReadLine();

            Debug.Log("line<" + line_index + ">: " + line + "\n" + "readmode: " + reading_mode.ToString());

            // 読み込んだパズルに問題があった場合、読み込みを停止する
            int result = readPuzzleInfo(line, line_index);
            if (result < 0){
                Debug.LogError(result);
                break;
            }
            line_index++;
        }
        data_reader.Close();
    }

    // Puzzle.cs から呼出
    public void init(string stage_name)
    {
        readPuzzle(stage_name);

        ui_manager = GetComponent<UiManager>();
        ui_manager.init(puzzle_total_num);
    }
}
