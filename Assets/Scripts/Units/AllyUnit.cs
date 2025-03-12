using UnityEngine;
using System.Collections;

public class AllyUnit : Unit
{
    public bool isStationary = true; // true이면 제자리, false이면 이동형
    public float trackingSpeed = 3f;   // 이동형일 때 적 추적 속도

    void Update()
    {
        if (isStationary)
        {
            // 제자리에서 적을 감지하고 공격
            FindAndAttackEnemy();
        }
        else
        {
            // 이동형: 가장 가까운 적을 찾아 추적 후 공격
            EnemyUnit closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                // 적을 향해 이동
                Vector3 direction = (closestEnemy.transform.position - transform.position).normalized;
                transform.position = Vector3.MoveTowards(transform.position, closestEnemy.transform.position, trackingSpeed * Time.deltaTime);

                // 적을 바라보도록 회전
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
                }

                // 일정 거리 내에 도달하면 공격
                if (Vector3.Distance(transform.position, closestEnemy.transform.position) <= attackRange)
                {
                    Attack(closestEnemy);
                }
            }
        }
    }

    // 제자리 공격용: 범위 내 적 감지 후 공격
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

    // 가장 가까운 적 유닛을 반환
    EnemyUnit FindClosestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange * 2f); // 조금 넓은 범위
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
            Debug.Log($"{gameObject.name}이(가) {target.name}을(를) 공격했습니다.");
        }
    }
}
