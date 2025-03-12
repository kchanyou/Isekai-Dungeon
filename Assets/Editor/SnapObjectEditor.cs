using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(SnapObject))]
public class SnapObjectEditor : Editor
{
    void OnSceneGUI()
    {
        SnapObject snapObject = (SnapObject)target;
        GridManager gridManager = snapObject.gridManager;
        if (gridManager == null)
        {
            gridManager = FindObjectOfType<GridManager>();
            snapObject.gridManager = gridManager;
        }

        Vector3 currentPos = snapObject.transform.position;
        Vector2Int gridPos = gridManager.GetGridPosition(currentPos);
        Vector3 snappedPos;

        if (snapObject.CompareTag("BaseBlock"))
        {
            // BaseBlock�� X, Z���� �׸��忡 ���߰�, Y���� ���� Y���� ���� ����� ������ ����
            float snappedY = Mathf.Round(currentPos.y);
            snappedPos = new Vector3(gridPos.x * gridManager.cellSize, snappedY, gridPos.y * gridManager.cellSize);
        }
        else
        {
            // AddedBlock�� ������ GetPlacementPosition�� ���
            snappedPos = gridManager.GetPlacementPosition(gridPos, "AddedBlock");
        }

        if (snapObject.transform.position != snappedPos)
        {
            Undo.RecordObject(snapObject.transform, "Snap To Grid");
            snapObject.transform.position = snappedPos;
        }
    }
}
