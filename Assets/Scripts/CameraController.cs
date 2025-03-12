using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private void Update()
    {
        // ī�޶� �̵��� ���� ��尡 CameraMove�� ���� �۵�
        if (GameManager.Instance.currentMode != GameMode.CameraMove)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(h, 0, v) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }
}
