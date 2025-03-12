using UnityEngine;

public class SnapObject : MonoBehaviour
{
    public GridManager gridManager;

    void Start()
    {
        if (gridManager == null)
            gridManager = FindObjectOfType<GridManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
            SnapToGrid();
    }

    void SnapToGrid()
    {
        if (gridManager != null)
        {
            Vector2Int gridPos = gridManager.GetGridPosition(transform.position);
            string objectTag = gameObject.tag;
            if (CompareTag("BaseBlock"))
            {
                transform.position = new Vector3(gridPos.x * gridManager.cellSize, transform.position.y, gridPos.y * gridManager.cellSize);
            }
            else
            {
                transform.position = gridManager.GetPlacementPosition(gridPos, "AddedBlock");
            }
        }
    }
}
