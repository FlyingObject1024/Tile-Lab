using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System.IO;
using System;
using TMPro;

public class Stage_select : MonoBehaviour
{

    int mode = 0;
    static string stages_file_path = "Assets/Data/stages.csv";
    static FileStream data_file = new FileStream(stages_file_path, FileMode.Open, FileAccess.Read);
    static StreamReader data_reader = new StreamReader(data_file);

    float camera_screen_height = 0.0f;
    float camera_screen_width = 0.0f;

    int block_image_height = 20;
    int block_image_width  = 20;

    float block_height = 1.0f;
    float block_width  = 1.0f;

    int x = 0;
    int y = 0;
    static int writing_line = 0;

    List<GameObject> stageList = new List<GameObject>();
    int stage_count = 0;

    void setSize(string line){
        string[] parts = line.Split(",");
        x = int.Parse(parts[1]);
        y = int.Parse(parts[3]);
    }

    void initStageButton(string stage_name, int x, int y){
        GameObject stage = new GameObject("stage<" + int.Parse(stage_name) + ">");
        stage.SetActive(true);

        /* �摜�ǉ� */
        // �X�v���C�g�����_���[�ǉ�
        SpriteRenderer renderer = stage.AddComponent<SpriteRenderer>();

        // Resources/Images/block��ǂݍ���
        string sprite_path = "Images/block";
        Sprite stageSprite = Resources.Load<Sprite>(sprite_path);
        if (stageSprite == null){
            Debug.LogError("Sprite not found at " + sprite_path);
        }
        else{
            renderer.sprite = stageSprite;
        }

        /* �e�L�X�g�ǉ� */
        // �e�L�X�g�\���p�̎q�I�u�W�F�N�g���쐬
        GameObject textObj = new GameObject("Label");
        textObj.transform.SetParent(stage.transform);

        // �e�L�X�g�̈ʒu�𒲐��i�X�v���C�g�̒����ɏd�˂�j
        textObj.transform.localPosition = new Vector3(0, 0, -0.1f); // Z���������ɂ��ăX�v���C�g���O�ɏo��

        // TextMesh�R���|�[�l���g��ǉ�
        TextMesh textMesh = textObj.AddComponent<TextMesh>();
        textMesh.text = stage_name;
        textMesh.fontSize = 100;
        textMesh.characterSize = 0.1f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.color = Color.black;

        // �R���C�_�[�ǉ�
        stage.AddComponent<BoxCollider2D>();

        // �N���b�N�n���h���ǉ�
        StageClickHandler clickHandler = stage.AddComponent<StageClickHandler>();
        clickHandler.stageName = stage_name;
        clickHandler.manager = FindObjectOfType<Change_scene>(); // �V�[����� StageManager ��T���ēn��
        
        // �\���ʒu��ݒ�iZ���̓J�����ɉf��ʒu�Ɂj
        stage.transform.position = new Vector3(y*block_width - (camera_screen_width / 2.0f), -(x * block_height - (camera_screen_height / 2.0f)), -1);

        // ���X�g�ɒǉ�
        stageList.Add(stage);
        stage_count++;
    }

    void generateStageSelector(string line){
        string[] parts = line.Split(",");

        Debug.Log(writing_line);

        for (int i = 0; i < parts.Length; i++){
            string part = parts[i];
            if(part != ""){
                initStageButton(part, writing_line, i);
            }
        }
        writing_line++;
    }

    void readStages(){
        int line_index = 0;

        while(data_reader.Peek() != -1){
            string line = data_reader.ReadLine();

            Debug.Log("line<" + line_index + ">: "+line);

            if      (line_index == 0) setSize(line);
            else if (line_index  > 0) generateStageSelector(line);

            line_index++;
            if (line_index >= y) break;
        }
        data_reader.Close();
    }

    void initAxisInfo(){
        Debug.Log("��ʂ̍����̍��W�� " + Camera.main.ScreenToWorldPoint(new Vector2(0, 0)));
        Debug.Log("��ʂ̉E��̍��W�� " + Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)));
        Debug.Log(Screen.width + "," + Screen.height);

        camera_screen_width  = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height))[0] - Camera.main.ScreenToWorldPoint(new Vector2(0, 0))[0];
        camera_screen_height = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height))[1] - Camera.main.ScreenToWorldPoint(new Vector2(0, 0))[1];
        Debug.Log(camera_screen_width + ", " + camera_screen_height);

        //block_width  = Camera.main.ScreenToWorldPoint(new Vector2(block_image_width, block_image_height))[0];
        //block_height = Camera.main.ScreenToWorldPoint(new Vector2(block_image_width, block_image_height))[1];
    }

    void Awake(){
        Debug.Log("Awake Stage_select");
        initAxisInfo();
        readStages();
        Debug.Log("mode: " + mode);
    }

    void Start(){

    }
}
