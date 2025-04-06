using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{

    float camera_screen_height = 0.0f;
    float camera_screen_width = 0.0f;

    Vector2 topRight;
    Vector2 bottomRight;
    Vector2 topLeft;
    Vector2 bottomLeft;

    public float inventory_field_height_level = 5;
    public float goal_field_height_level = 5;
    public float field_margin = 0.0f;

    float inventory_field_width;
    float inventory_field_height;

    float goal_field_width;
    float goal_field_height;


    public GameObject uiCanvas;

    public GameObject inventoryField;
    public GameObject inventoryField_scrollView;

    public GameObject goalField;
    public GameObject goalField_scrollView;

    public GameObject progressField;
    public Sprite circleSprite;
    List<GameObject> progress_circles = new List<GameObject> ();


    void initAxisInfo()
    {
        Camera cam = Camera.main;

        camera_screen_width = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height))[0] - Camera.main.ScreenToWorldPoint(new Vector2(0, 0))[0];
        camera_screen_height = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height))[1] - Camera.main.ScreenToWorldPoint(new Vector2(0, 0))[1];

        bottomLeft  = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        topLeft     = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height));
        bottomRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0));
        topRight    = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        Debug.Log(camera_screen_width + ", " + camera_screen_height);
        Debug.Log("TopLeft: "     + topLeft);
        Debug.Log("BottomLeft: "  + bottomLeft);
        Debug.Log("TopRight: "    + topRight);
        Debug.Log("BottomRight: " + bottomRight);
    }
    
    void initProgressCircle(int puzzle_total_num)
    {
        // フィールドを横幅1/5にする作業をやる(未)
        float progress_field_width = Screen.width / 5.0f;
        float size = 5.0f;    // 円のサイズ（スケール）
        float spacing = 1.2f; // 間隔

        for (int i = 0; i < puzzle_total_num; i++)
        {
            // 外側の黒い枠
            GameObject border = new GameObject("circleBorder<" + (i + 1) + "/" + puzzle_total_num + ">");
            SpriteRenderer borderRenderer = border.AddComponent<SpriteRenderer>();

            borderRenderer.sprite = circleSprite;
            borderRenderer.color = Color.black;
            border.transform.localScale = new Vector3(size * 1.2f, size * 1.2f, 1);

            // 内側の円
            GameObject circle = new GameObject("progressCircle<" + (i + 1) + "/" + puzzle_total_num + ">");
            RectTransform circle_rt = circle.GetComponent<RectTransform>(); 
            SpriteRenderer renderer = circle.AddComponent<SpriteRenderer>();
            renderer.sprite = circleSprite;
            if (i == 0)
            {
                renderer.color = Color.white;
            }
            else
            {
                renderer.color = Color.black;
            }
            circle.transform.localScale = new Vector3(size, size, 1);

            // 配置位置
            Vector2 position = topLeft + new Vector2(i * spacing + spacing, -spacing);
            circle.transform.position = position;
            border.transform.position = position;

            RectTransform progressRectTransform = progressField.GetComponent<RectTransform>();

            // 枠の上に円を置く
            circle.transform.parent = progressRectTransform;
            border.transform.parent = circle.transform;

            progress_circles.Add(circle);
        }
    }
    
    void initProgressField(int puzzle_total_num)
    {
        initProgressCircle(puzzle_total_num);

        RectTransform progressRectTransform = progressField.GetComponent<RectTransform>();
        progressRectTransform.anchorMin = new Vector2(0f, 1f);
        progressRectTransform.anchorMax = new Vector2(0f, 1f);
        progressRectTransform.anchoredPosition = new Vector2(0f, 0f);
    }

    void initInventoryFieldScrollView()
    {
        // 高さ・幅調整
        inventory_field_width  =  Screen.width                                  - field_margin * 2.0f;
        inventory_field_height = (Screen.height / inventory_field_height_level) - field_margin * 2.0f;

        Transform scrollViewTransform = inventoryField_scrollView.GetComponent<RectTransform>();
        if (scrollViewTransform != null)
        {
            RectTransform rt = scrollViewTransform.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = new Vector2(0f, inventory_field_height/2.0f + field_margin);
                rt.sizeDelta = new Vector2(inventory_field_width, inventory_field_height);

                Debug.Log($"ScrollView resized to: {inventory_field_width}x{inventory_field_height}");
            }
            else
            {
                Debug.LogWarning("ScrollView does not have RectTransform.");
            }
        }
        else
        {
            Debug.LogWarning("ScrollView not found under InventoryField.");
        }
    }

    void initInventoryField()
    {
        initInventoryFieldScrollView();

        RectTransform inventoryRectTransform = inventoryField.GetComponent<RectTransform>();
        inventoryRectTransform.anchorMin = new Vector2(0.5f, 0f);
        inventoryRectTransform.anchorMax = new Vector2(0.5f, 0f);
        inventoryRectTransform.anchoredPosition = new Vector2(0f, 0f);
    }

    void initGoalFieldScrollView()
    {
        // 高さ・幅調整
        goal_field_width  = Screen.width - Screen.width / 5.0f - field_margin * 2.0f;
        goal_field_height = (Screen.height / goal_field_height_level) - field_margin * 2.0f;

        Transform scrollViewTransform = goalField_scrollView.GetComponent<RectTransform>();
        if (scrollViewTransform != null)
        {
            RectTransform rt = scrollViewTransform.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = new Vector2((-goal_field_width / 2.0f) - field_margin, (-goal_field_height / 2.0f) - field_margin);
                rt.sizeDelta = new Vector2(goal_field_width, goal_field_height);

                Debug.Log($"ScrollView resized to: {goal_field_width}x{goal_field_height}");
            }
            else
            {
                Debug.LogWarning("ScrollView does not have RectTransform.");
            }
        }
        else
        {
            Debug.LogWarning("ScrollView not found under GoalField.");
        }
    }

    void initGoalField()
    {
        initGoalFieldScrollView();

        RectTransform goalRectTransform = goalField.GetComponent<RectTransform>();
        goalRectTransform.anchorMin = new Vector2(1f, 1f);
        goalRectTransform.anchorMax = new Vector2(1f, 1f);
        goalRectTransform.anchoredPosition = new Vector2(0f, 0f);
    }

    // StageManager.cs から呼出
    public void init(int puzzle_total_num)
    {
        initAxisInfo();
        initProgressField(puzzle_total_num);

        initInventoryField();
        initGoalField();
    }
}
