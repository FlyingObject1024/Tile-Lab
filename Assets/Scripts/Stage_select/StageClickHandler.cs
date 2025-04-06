using UnityEngine;

public class StageClickHandler : MonoBehaviour
{
    // ステージ名（数値）
    public string stageName;

    // 外部スクリプトの参照
    public Change_scene manager;

    void OnMouseDown()
    {
        if (manager != null)
        {
            manager.change_to_puzzle_scene(stageName); // 外部スクリプトの関数を呼ぶ
        }
        else
        {
            Debug.LogWarning("StageManager が設定されていません");
        }
    }
}
