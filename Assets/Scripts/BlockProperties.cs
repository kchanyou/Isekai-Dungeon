using UnityEngine;

public class BlockProperties : MonoBehaviour
{
    public float blockHeight = 1f;

    private void Start()
    {
        blockHeight = gameObject.transform.position.y;
    }
}
