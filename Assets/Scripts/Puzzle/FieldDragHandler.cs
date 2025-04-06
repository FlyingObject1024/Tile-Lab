using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldDragHandler : MonoBehaviour
{
    public Camera camera;

    private Vector3 lastMousePosition;

    void OnMouseDown()
    {
        // マウスの最初の位置を記録
        lastMousePosition = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        // 現在のマウス位置
        Vector3 currentMousePosition = Input.mousePosition;

        // マウスの移動量
        Vector3 delta = currentMousePosition - lastMousePosition;

        // カメラを移動（スクリーン座標の差分をワールド座標に変換）
        Vector3 move = camera.ScreenToWorldPoint(new Vector3(currentMousePosition.x, currentMousePosition.y, camera.transform.position.z))
                     - camera.ScreenToWorldPoint(new Vector3(lastMousePosition.x, lastMousePosition.y, camera.transform.position.z));

        camera.transform.position -= new Vector3(move.x, move.y, 0);  // Zは固定

        // マウス位置を更新
        lastMousePosition = currentMousePosition;
    }
}
