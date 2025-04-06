using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Unityエンジンのシーン管理プログラムを利用する

public class Change_scene : MonoBehaviour
{
    string[] scenes = { "Title", "Stage_select", "Puzzle" };
    public static int active_sceneid = 0;

    string[] modes = { "Mono Color", "Full Color", "Factry" };
    public static int active_modeid = 0;

    public static string active_stage_name = "";

    public string getActive_stage_name()
    {
        return active_stage_name;
    }

    public void load_entry_scene()
    {
        active_sceneid = 0;
        Debug.Log("Title Scene");
        SceneManager.LoadScene(scenes[active_sceneid], LoadSceneMode.Additive);
    }

    public void change_to_title_scene()
    {
        active_sceneid = 0;
        Debug.Log("Title Scene");
        SceneManager.LoadScene(scenes[active_sceneid]);
    }

    public void change_to_mono_color_scene()
    {
        active_sceneid = 1;
        active_modeid = 0;
        Debug.Log("Stage_select Scene<" + modes[active_modeid] + ">");
        SceneManager.LoadScene(scenes[active_sceneid]);
    }

    public void change_to_puzzle_scene(string stagename)
    {
        active_sceneid = 2;
        Debug.Log("Puzzle Scene<stage: " + stagename + ">");
        active_stage_name = stagename;
        SceneManager.LoadScene(scenes[active_sceneid]);
    }

    public void print_stage_name()
    {
        Debug.Log(active_stage_name);
    }

    public void back_scene()
    {
        if (active_sceneid != 0)
        {
            active_sceneid--;
            SceneManager.LoadScene(scenes[active_sceneid]);
        }
    }
}