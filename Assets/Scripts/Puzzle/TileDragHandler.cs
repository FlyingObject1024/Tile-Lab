using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDragHandler : MonoBehaviour
{
    private TileManager tile_mng;
    public bool isDragging = false;

    void Start()
    {
        tile_mng = GetComponent<TileManager>();
        if (tile_mng == null)
        {
            Debug.LogError("tilemanager is null");
            return;
        }
    }

    void OnMouseDown()
    {
        if (tile_mng.isDraggable)
        {
            tile_mng.RemoveFromGrid();
            isDragging = true;
        }
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos;
        }
    }

    void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;

        Vector2Int gridPos = GridManager.Instance.WorldToGrid(transform.position);

        // gridñ¢êËóLÇ»ÇÁîzíu
        if (!GridManager.Instance.IsOccupied(gridPos))
        {
            tile_mng.PlaceAt(gridPos);
        }
        else
        {
            // ñﬂÇ∑
            tile_mng.PlaceAt(tile_mng.gridPosition);
        }
        
    }
}
