using UnityEngine;

public class Trap : Unit
{
    void Update()
    {
        // 범위 내 적 탐색
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
            Debug.Log($"{gameObject.name}이(가) {target.name}을(를) 공격했습니다.");
        }
    }
}
