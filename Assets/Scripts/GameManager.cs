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

    [Header("현재 모드")]
    public GameMode currentMode = GameMode.CameraMove;

    [Header("참조할 매니저")]
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

        // 인스펙터에서 할당하지 않은 경우, FindObjectOfType()를 사용할 수 있습니다.
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
        // 예시: Q키로 모드 전환
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
        Debug.Log("모드 전환: " + currentMode);
    }
}
