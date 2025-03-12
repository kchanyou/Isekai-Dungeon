using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public float cellSize = 1f;
    public int gridWidth = 10;
    public int gridHeight = 10;

    // 각 셀에 배치된 블록 목록 (키: 격자 좌표)
    private Dictionary<Vector2Int, List<GameObject>> occupancy = new Dictionary<Vector2Int, List<GameObject>>();

    public Vector2Int GetGridPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / cellSize);
        int z = Mathf.RoundToInt(position.z / cellSize);
        return new Vector2Int(x, z);
    }

    public List<GameObject> GetCellObjects(Vector2Int gridPos)
    {
        if (!occupancy.ContainsKey(gridPos))
            occupancy[gridPos] = new List<GameObject>();
        return occupancy[gridPos];
    }

    /// <summary>
    /// 오브젝트 타입에 따라 해당 셀의 배치 위치를 계산합니다.
    /// - "BaseBlock": 셀 내 BaseBlock의 개수를 세어, 새 BaseBlock은 (baseCount * cellSize)의 높이에 배치.
    /// - "AddedBlock": 셀에 BaseBlock이 있어야 하며, 추가 블록 수에 따라 (1 + addedCount) * cellSize 높이에 배치.
    /// </summary>
    public Vector3 GetPlacementPosition(Vector2Int gridPos, string objectTag)
    {
        if (objectTag == "BaseBlock")
        {
            // BaseBlock은 항상 y=0에 배치 (바닥 구성)
            return new Vector3(gridPos.x * cellSize, 0, gridPos.y * cellSize);
        }
        else if (objectTag == "AddedBlock")
        {
            List<GameObject> cellObjects = GetCellObjects(gridPos);
            bool hasBase = false;
            int addedCount = 0;
            foreach (GameObject obj in cellObjects)
            {
                if (obj.CompareTag("BaseBlock"))
                    hasBase = true;
                else if (obj.CompareTag("AddedBlock"))
                    addedCount++;
            }
            if (hasBase)
            {
                float y = (1 + addedCount) * cellSize;
                return new Vector3(gridPos.x * cellSize, y, gridPos.y * cellSize);
            }
            else
            {
                // BaseBlock이 없는 셀은 추가 배치 불가능하므로 y=0 반환
                return new Vector3(gridPos.x * cellSize, 0, gridPos.y * cellSize);
            }
        }
        else
        {
            float y = GetCellObjects(gridPos).Count * cellSize;
            return new Vector3(gridPos.x * cellSize, y, gridPos.y * cellSize);
        }
    }

    // 기존 GetWorldPosition (참고용)
    public Vector3 GetWorldPosition(Vector2Int gridPos)
    {
        float y = GetCellObjects(gridPos).Count * cellSize;
        return new Vector3(gridPos.x * cellSize, y, gridPos.y * cellSize);
    }

    public void PlaceBlock(Vector2Int gridPos, GameObject block)
    {
        GetCellObjects(gridPos).Add(block);
    }

    public GameObject RemoveTopBlock(Vector2Int gridPos)
    {
        List<GameObject> cellObjects = GetCellObjects(gridPos);
        if (cellObjects.Count > 0)
        {
            GameObject top = cellObjects[cellObjects.Count - 1];
            cellObjects.RemoveAt(cellObjects.Count - 1);
            return top;
        }
        return null;
    }
    public bool CanPlaceBlock(Vector2Int gridPos)
    {
        List<GameObject> cellObjects = GetCellObjects(gridPos);
        bool hasBase = false;
        foreach (GameObject obj in cellObjects)
        {
            if (obj.CompareTag("BaseBlock"))
            {
                hasBase = true;
                break;
            }
        }
        // BaseBlock이 없는 셀은 추가 배치가 불가능합니다.
        if (!hasBase)
            return false;

        // 한 셀에 BaseBlock과 추가 블록을 포함해 최대 2개까지만 허용합니다.
        return cellObjects.Count < 2;
    }
    public void RecalculateOccupancy()
    {
        occupancy.Clear();
        GameObject[] baseBlocks = GameObject.FindGameObjectsWithTag("BaseBlock");
        foreach (GameObject obj in baseBlocks)
        {
            Vector2Int pos = GetGridPosition(obj.transform.position);
            GetCellObjects(pos).Add(obj);
        }
        GameObject[] addedBlocks = GameObject.FindGameObjectsWithTag("AddedBlock");
        foreach (GameObject obj in addedBlocks)
        {
            Vector2Int pos = GetGridPosition(obj.transform.position);
            GetCellObjects(pos).Add(obj);
        }
    }

    private void Awake()
    {
        RecalculateOccupancy();
    }
}
