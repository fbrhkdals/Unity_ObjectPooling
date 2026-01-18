using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("이동/공격 속성")]
    public float moveSpeed = 2f;
    public float attackDamage = 5f;
    public float attackRange = 2f;       // 아군 유닛 공격 범위
    public float towerAttackRange = 1.5f;// 아군 타워 공격 범위
    public float attackInterval = 1f;

    [Header("체력")]
    public float maxHp = 20f;
    private float currentHp;

    private PlayerBase playerBase;
    private float attackTimer;

    void Start()
    {
        currentHp = maxHp;
        playerBase = Object.FindFirstObjectByType<PlayerBase>();
    }

    void Update()
    {
        PlayerUnit targetUnit = FindClosestPlayerUnit();
        float distanceToUnit = targetUnit != null ? Vector2.Distance(transform.position, targetUnit.transform.position) : Mathf.Infinity;
        float distanceToTower = playerBase != null ? Vector2.Distance(transform.position, playerBase.transform.position) : Mathf.Infinity;

        bool canAttackUnit = targetUnit != null && distanceToUnit <= attackRange;
        bool canAttackTower = playerBase != null && distanceToTower <= towerAttackRange;

        if (!canAttackUnit && !canAttackTower)
        {
            // 공격할 대상 없으면 왼쪽으로 이동
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            attackTimer = 0f; // 공격 루프 초기화
        }
        else
        {
            // 공격 루프
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                attackTimer = 0f;

                if (canAttackUnit)
                    targetUnit.TakeDamage(attackDamage);
                else if (canAttackTower)
                    playerBase.TakeDamage(attackDamage);
            }
        }
    }

    PlayerUnit FindClosestPlayerUnit()
    {
        PlayerUnit[] units = Object.FindObjectsByType<PlayerUnit>(UnityEngine.FindObjectsSortMode.None);
        PlayerUnit closest = null;
        float minDist = Mathf.Infinity;

        foreach (PlayerUnit u in units)
        {
            float dist = Vector2.Distance(transform.position, u.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = u;
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
