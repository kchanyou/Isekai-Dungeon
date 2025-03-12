using UnityEngine;
using System.Collections;

public class AllyUnit : Unit
{
    public bool isStationary = true; // true�̸� ���ڸ�, false�̸� �̵���
    public float trackingSpeed = 3f;   // �̵����� �� �� ���� �ӵ�

    void Update()
    {
        if (isStationary)
        {
            // ���ڸ����� ���� �����ϰ� ����
            FindAndAttackEnemy();
        }
        else
        {
            // �̵���: ���� ����� ���� ã�� ���� �� ����
            EnemyUnit closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                // ���� ���� �̵�
                Vector3 direction = (closestEnemy.transform.position - transform.position).normalized;
                transform.position = Vector3.MoveTowards(transform.position, closestEnemy.transform.position, trackingSpeed * Time.deltaTime);

                // ���� �ٶ󺸵��� ȸ��
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
                }

                // ���� �Ÿ� ���� �����ϸ� ����
                if (Vector3.Distance(transform.position, closestEnemy.transform.position) <= attackRange)
                {
                    Attack(closestEnemy);
                }
            }
        }
    }

    // ���ڸ� ���ݿ�: ���� �� �� ���� �� ����
    void FindAndAttackEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                EnemyUnit enemy = col.GetComponent<EnemyUnit>();
                if (enemy != null)
                {
                    Attack(enemy);
                    return;
                }
            }
        }
    }

    // ���� ����� �� ������ ��ȯ
    EnemyUnit FindClosestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange * 2f); // ���� ���� ����
        EnemyUnit closest = null;
        float minDist = Mathf.Infinity;
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                float dist = Vector3.Distance(transform.position, col.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = col.GetComponent<EnemyUnit>();
                }
            }
        }
        return closest;
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
