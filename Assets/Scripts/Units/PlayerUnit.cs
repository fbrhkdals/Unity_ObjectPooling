using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    [Header("이동/공격 속성")]
    public float moveSpeed = 2f;        // 이동 속도
    public float attackDamage = 5f;     // 공격 피해
    public float attackRange = 2f;      // 유닛 공격 범위
    public float towerAttackRange = 1.5f;// 타워 공격 범위
    public float attackInterval = 1f;   // 공격 속도 (초)

    [Header("체력")]
    public float maxHp = 20f;
    private float currentHp;

    private float attackTimer;
    private EnemyBase enemyBase;

    void Start()
    {
        currentHp = maxHp;
        enemyBase = Object.FindFirstObjectByType<EnemyBase>();
    }

    void Update()
    {
        Enemy targetEnemy = FindClosestEnemy();
        float distanceToEnemy = targetEnemy != null ? Vector2.Distance(transform.position, targetEnemy.transform.position) : Mathf.Infinity;
        float distanceToTower = enemyBase != null ? Vector2.Distance(transform.position, enemyBase.transform.position) : Mathf.Infinity;

        bool canAttackEnemy = targetEnemy != null && distanceToEnemy <= attackRange;
        bool canAttackTower = enemyBase != null && distanceToTower <= towerAttackRange;

        if (!canAttackEnemy && !canAttackTower)
        {
            // 공격할 대상 없으면 이동
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            attackTimer = 0f; // 공격 루프 초기화
        }
        else
        {
            // 공격 루프
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                attackTimer = 0f;

                if (canAttackEnemy)
                    targetEnemy.TakeDamage(attackDamage);
                else if (canAttackTower)
                    enemyBase.TakeDamage(attackDamage);
            }
        }
    }

    Enemy FindClosestEnemy()
    {
        Enemy[] enemies = Object.FindObjectsByType<Enemy>(UnityEngine.FindObjectsSortMode.None);
        Enemy closest = null;
        float minDist = Mathf.Infinity;

        foreach (Enemy e in enemies)
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = e;
            }
        }

        return closest;
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
            Destroy(gameObject);
    }
}
