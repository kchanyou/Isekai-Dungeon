using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float floatSpeed = 1f;   // 위아래 움직임 속도
    public float floatAmount = 0.5f; // 움직이는 높이 범위
    public float horizontalSpeed = 0.5f; // 좌우 움직임 속도
    public float horizontalAmount = 0.2f; // 좌우 움직이는 범위

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // 시작 위치 저장
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmount; // 위아래 움직임
        float xOffset = Mathf.Cos(Time.time * horizontalSpeed) * horizontalAmount; // 좌우 움직임

        transform.position = startPos + new Vector3(xOffset, yOffset, 0);
    }
}
