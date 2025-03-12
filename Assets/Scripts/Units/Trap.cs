using UnityEngine;

public class Trap : Unit
{
    void Update()
    {
        // ���� �� �� Ž��
        Collider[] enemies = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Attack(enemy.GetComponent<EnemyUnit>());
                return;
            }
        }
    }

    void Attack(EnemyUnit target)
    {
        if (target != null)
        {
            target.TakeDamage(attackPower);
            Debug.Log($"{gameObject.name}��(��) {target.name}��(��) �����߽��ϴ�.");
        }
    }
}
