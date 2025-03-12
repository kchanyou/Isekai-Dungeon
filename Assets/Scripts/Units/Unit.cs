using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public int health;
    public float attackRange;
    public float movementSpeed;
    public string unitType; // ��: "Fire", "Water" ��
    public int attackPower;
    public int defensePower;

    public virtual void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - defensePower, 1);
        health -= finalDamage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name}��(��) ����߽��ϴ�.");
        Destroy(gameObject);
    }
}
