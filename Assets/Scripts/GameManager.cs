using System;
using UnityEngine;

public enum GameMode
{
    CameraMove,
    Placement,
    Removal
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("���� ���")]
    public GameMode currentMode = GameMode.CameraMove;

    [Header("������ �Ŵ���")]
    public ResourceManager resourceManager;
    public PlacementManager placementManager;
    public RemovalManager removalManager;
    public CameraController cameraController;

    public event Action<GameMode> OnModeChanged;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // �ν����Ϳ��� �Ҵ����� ���� ���, FindObjectOfType()�� ����� �� �ֽ��ϴ�.
        if (resourceManager == null)
            resourceManager = FindObjectOfType<ResourceManager>();
        if (placementManager == null)
            placementManager = FindObjectOfType<PlacementManager>();
        if (removalManager == null)
            removalManager = FindObjectOfType<RemovalManager>();
        if (cameraController == null)
            cameraController = FindObjectOfType<CameraController>();
    }

    private void Update()
    {
        // ����: QŰ�� ��� ��ȯ
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchMode();
        }
    }

    public void SwitchMode()
    {
        if (currentMode == GameMode.CameraMove)
            currentMode = GameMode.Placement;
        else if (currentMode == GameMode.Placement)
            currentMode = GameMode.Removal;
        else
            currentMode = GameMode.CameraMove;

        OnModeChanged?.Invoke(currentMode);
        Debug.Log("��� ��ȯ: " + currentMode);
    }
}
