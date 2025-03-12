using UnityEngine;
using System.Collections;

public class EnemyUnit : Unit
{
    public Transform target; // 최종 목표(예: 포탈, 기지 등)
    public float stoppingDistance = 0.5f; // 목표 도달 거리
    private bool isFighting = false;

    void Update()
    {
        if (!isFighting)
        {
            MoveToTarget();
            CheckForEnemies();
        }
    }

    // 목적지까지 직선으로 이동 (장애물 회피 없음)
    void MoveToTarget()
    {
        if (target != null)
        {
            // 목표 방향 계산 및 회전
            Vector3 direction = (target.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
            }

            // 목표와의 거리가 충분히 멀면 이동
            if (Vector3.Distance(transform.position, target.position) > stoppingDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);
            }
        }
    }

    // 범위 내에 있는 아군 유닛이나 트랩을 감지하여 전투 시작
    void CheckForEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Ally") || col.CompareTag("Trap"))
            {
                Unit targetUnit = col.GetComponent<Unit>();
                if (targetUnit != null)
                {
                    StartCoroutine(EngageCombat(targetUnit));
                    return;
                }
            }
        }
    }

    IEnumerator EngageCombat(Unit targetUnit)
    {
        if (targetUnit == null) yield break;

        isFighting = true;
        // 전투 중에는 이동하지 않음

        while (targetUnit != null && targetUnit.health > 0)
        {
            targetUnit.TakeDamage(attackPower);
            Debug.Log($"{gameObject.name}이(가) {targetUnit.name}을(를) 공격 중입니다.");
            yield return new WaitForSeconds(1f);
        }

        isFighting = false;
        // 전투가 끝나면 다시 목표로 이동
    }
}
