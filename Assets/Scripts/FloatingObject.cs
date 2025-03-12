using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float floatSpeed = 1f;   // ���Ʒ� ������ �ӵ�
    public float floatAmount = 0.5f; // �����̴� ���� ����
    public float horizontalSpeed = 0.5f; // �¿� ������ �ӵ�
    public float horizontalAmount = 0.2f; // �¿� �����̴� ����

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // ���� ��ġ ����
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmount; // ���Ʒ� ������
        float xOffset = Mathf.Cos(Time.time * horizontalSpeed) * horizontalAmount; // �¿� ������

        transform.position = startPos + new Vector3(xOffset, yOffset, 0);
    }
}
