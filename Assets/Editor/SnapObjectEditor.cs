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
            // BaseBlock은 X, Z축은 그리드에 맞추고, Y축은 현재 Y값의 가장 가까운 정수로 스냅
            float snappedY = Mathf.Round(currentPos.y);
            snappedPos = new Vector3(gridPos.x * gridManager.cellSize, snappedY, gridPos.y * gridManager.cellSize);
        }
        else
        {
            // AddedBlock은 기존의 GetPlacementPosition을 사용
            snappedPos = gridManager.GetPlacementPosition(gridPos, "AddedBlock");
        }

        if (snapObject.transform.position != snappedPos)
        {
            Undo.RecordObject(snapObject.transform, "Snap To Grid");
            snapObject.transform.position = snappedPos;
        }
    }
}
