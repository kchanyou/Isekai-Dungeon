using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class RemovalManager : MonoBehaviour
{
    public GridManager gridManager;
    public ResourceManager resourceManager;
    public Camera mainCamera;
    public GameObject removalParticlePrefab;

    private GameObject currentHighlightedBlock = null;
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();

    private void Start()
    {
        if (gridManager == null)
            gridManager = FindObjectOfType<GridManager>();
        if (resourceManager == null)
            resourceManager = FindObjectOfType<ResourceManager>();
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        if (GameManager.Instance.currentMode != GameMode.Removal)
            return;
        HandleRemoval();
    }

    void HandleRemoval()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float distance;
        if (ground.Raycast(ray, out distance))
        {
            Vector3 worldPoint = ray.GetPoint(distance);
            Vector2Int gridPos = gridManager.GetGridPosition(worldPoint);
            List<GameObject> cellObjects = gridManager.GetCellObjects(gridPos);

            if (cellObjects.Count > 0)
            {
                GameObject topBlock = cellObjects[cellObjects.Count - 1];
                if (topBlock.CompareTag("BaseBlock"))
                {
                    UnhighlightBlock(currentHighlightedBlock);
                    currentHighlightedBlock = null;
                    Debug.Log("해당 블록은 제거할 수 없습니다.");
                    return;
                }
                if (currentHighlightedBlock != topBlock)
                {
                    UnhighlightBlock(currentHighlightedBlock);
                    HighlightBlock(topBlock);
                    currentHighlightedBlock = topBlock;
                }
            }
            else
            {
                UnhighlightBlock(currentHighlightedBlock);
                currentHighlightedBlock = null;
            }
        }

        if (Input.GetMouseButtonDown(0) && currentHighlightedBlock != null)
        {
            Vector2Int gridPos = gridManager.GetGridPosition(currentHighlightedBlock.transform.position);
            GameObject blockToRemove = gridManager.RemoveTopBlock(gridPos);
            if (blockToRemove != null)
            {
                string objectName = blockToRemove.name.Replace("(Clone)", "").Trim();
                int cost = resourceManager.GetObjectCost(objectName);
                if (cost >= 0)
                {
                    if (resourceManager.SpendGoldForObject(objectName))
                    {
                        PlayRemovalAnimation(blockToRemove);
                        Debug.Log($"{objectName} 제거됨 (비용: {cost})");
                    }
                    else
                    {
                        Debug.Log("골드가 부족하여 제거할 수 없습니다.");
                        gridManager.PlaceBlock(gridPos, blockToRemove);
                    }
                }
                else
                {
                    resourceManager.AddGold(-cost);
                    PlayRemovalAnimation(blockToRemove);
                    Debug.Log($"{objectName} 제거됨 (획득: {-cost})");
                }
            }
            currentHighlightedBlock = null;
        }
    }

    void PlayRemovalAnimation(GameObject block)
    {
        Sequence removalSeq = DOTween.Sequence();
        removalSeq.Append(block.transform.DOMoveY(block.transform.position.y + 0.5f, 0.2f).SetEase(Ease.OutQuad));
        removalSeq.Join(block.transform.DORotate(new Vector3(0, 360, 0), 0.3f, RotateMode.FastBeyond360).SetEase(Ease.Linear));
        removalSeq.Append(block.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack));
        removalSeq.OnComplete(() =>
        {
            if (removalParticlePrefab != null)
            {
                GameObject particleEffect = Instantiate(removalParticlePrefab, block.transform.position, Quaternion.identity);
                Destroy(particleEffect, 1.5f);
            }
            Destroy(block);
        });
    }

    void HighlightBlock(GameObject block)
    {
        if (block == null)
            return;
        Renderer rend = block.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            if (!originalColors.ContainsKey(block))
                originalColors[block] = rend.material.color;
            rend.material.color = Color.red;
        }
    }

    void UnhighlightBlock(GameObject block)
    {
        if (block == null)
            return;
        Renderer rend = block.GetComponentInChildren<Renderer>();
        if (rend != null && originalColors.ContainsKey(block))
        {
            rend.material.color = originalColors[block];
            originalColors.Remove(block);
        }
    }
}
