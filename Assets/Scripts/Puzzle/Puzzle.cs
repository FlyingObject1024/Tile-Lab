using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System.IO;
using System;
using TMPro;

public class Puzzle : MonoBehaviour
{
    public Change_scene manager;
    public StageManager stage_manager;

    [SerializeField]
    public string active_stage_name;

    void Awake()
    {
        Debug.Log("Awake Puzzle");
        manager      = GetComponent<Change_scene>();
        stage_manager = GetComponent<StageManager>();

        if (manager != null && stage_manager != null)
        {
            if (active_stage_name == null)
            {
                active_stage_name = manager.getActive_stage_name();
            }
            stage_manager.init(active_stage_name);
        }
        else
        {
            Debug.LogWarning("StageManager Ç™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒ");
        }
    }
}
