using DG.Tweening;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public GridManager gridManager;
    public ResourceManager resourceManager;
    public Camera mainCamera;
    public GameObject placementParticlePrefab;

    [Header("��� �ִϸ��̼� ����")]
    public float dropHeight = 5f;
    public float dropDuration = 0.5f;
    public float bounceDuration = 0.2f;

    [Header("��ġ ������ ������Ʈ��")]
    public GameObject[] objectPrefabs;
    public int selectedObjectIndex = 0;

    private GameObject previewInstance;
    private Renderer previewRenderer;
    private Material previewMaterial;
    private Color validColor = new Color(0, 1, 0, 0.5f);   // ��ġ ����: �ʷϻ� ������
    private Color invalidColor = new Color(1, 0, 0, 0.5f);  // ��ġ �Ұ���: ������ ������

    private void Start()
    {
        if (gridManager == null)
            gridManager = FindObjectOfType<GridManager>();
        if (resourceManager == null)
            resourceManager = FindObjectOfType<ResourceManager>();
        if (mainCamera == null)
            mainCamera = Camera.main;

        gridManager.RecalculateOccupancy();

        UpdatePreviewObject();
    }

    private void Update()
    {
        if (GameManager.Instance.currentMode == GameMode.Placement)
        {
            if (previewInstance != null)
                previewInstance.SetActive(true);
            HandlePlacement();
        }
        else
        {
            if (previewInstance != null)
                previewInstance.SetActive(false);
        }
    }

    void HandlePlacement()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float distance;
        if (!ground.Raycast(ray, out distance))
            return;

        Vector3 worldPoint = ray.GetPoint(distance);
        Vector2Int gridPos = gridManager.GetGridPosition(worldPoint);
        // ���õ� ������Ʈ�� �±׸� ����Ͽ� ��ġ ��ġ ���
        string selectedTag = objectPrefabs[selectedObjectIndex].tag;
        Vector3 targetPos = gridManager.GetPlacementPosition(gridPos, selectedTag);

        if (previewInstance != null)
        {
            previewInstance.transform.position = targetPos;
            previewRenderer.material.color = gridManager.CanPlaceBlock(gridPos) ? validColor : invalidColor;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (gridManager.CanPlaceBlock(gridPos))
            {
                string objectName = objectPrefabs[selectedObjectIndex].name;
                if (resourceManager.SpendGoldForObject(objectName))
                {
                    Vector3 startPos = targetPos + Vector3.up * dropHeight;
                    GameObject newBlock = Instantiate(objectPrefabs[selectedObjectIndex], startPos, Quaternion.identity);
                    newBlock.tag = selectedTag; // �±� �״�� �ο�
                    gridManager.PlaceBlock(gridPos, newBlock);

                    SnapObject snapObj = newBlock.GetComponent<SnapObject>();
                    if (snapObj != null)
                        Destroy(snapObj);

                    Sequence dropSeq = DOTween.Sequence();
                    dropSeq.Append(newBlock.transform.DOMove(targetPos, dropDuration).SetEase(Ease.InCubic));
                    dropSeq.Append(newBlock.transform.DOScale(new Vector3(1.2f, 0.8f, 1.2f), bounceDuration).SetEase(Ease.OutQuad));
                    dropSeq.Append(newBlock.transform.DOScale(Vector3.one, bounceDuration).SetEase(Ease.OutQuad));
                    if (placementParticlePrefab != null)
                    {
                        GameObject particleEffect = Instantiate(placementParticlePrefab, targetPos, Quaternion.identity);
                        Destroy(particleEffect, 1.5f);
                    }

                    Debug.Log($"�� {objectName} ��ġ��, ��ǥ ��ġ: {targetPos}");
                    GameManager.Instance.currentMode = GameMode.CameraMove;
                }
                else
                {
                    Debug.Log("��尡 �����մϴ�!");
                }
            }
            else
            {
                Debug.Log("�ش� ���� ��ġ�� �� �����ϴ�.");
            }
        }
    }

    public void UpdatePreviewObject()
    {
        if (previewInstance == null)
        {
            previewInstance = Instantiate(objectPrefabs[selectedObjectIndex]);
            previewInstance.transform.localScale *= 0.9f;
            Collider col = previewInstance.GetComponent<Collider>();
            if (col != null)
                col.enabled = false;
            previewRenderer = previewInstance.GetComponentInChildren<Renderer>();
            if (previewRenderer != null)
            {
                previewMaterial = previewRenderer.material;
                previewMaterial.color = validColor;
            }
        }
        else
        {
            GameObject newPreview = Instantiate(objectPrefabs[selectedObjectIndex]);
            MeshFilter mfNew = newPreview.GetComponent<MeshFilter>();
            MeshFilter mfOld = previewInstance.GetComponent<MeshFilter>();
            if (mfNew != null && mfOld != null)
                mfOld.mesh = mfNew.mesh;
            Destroy(newPreview);
        }
    }

    public void SelectObject(int index)
    {
        if (index >= 0 && index < objectPrefabs.Length)
        {
            selectedObjectIndex = index;
            UpdatePreviewObject();
            Debug.Log($"���õ� ������Ʈ: {objectPrefabs[index].name}");
        }
    }
}
