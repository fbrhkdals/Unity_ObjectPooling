using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float maxHp = 100f;
    public float currentHp;

    void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        Debug.Log("Enemy Base HP: " + currentHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("WIN");
        // 나중에 게임 오버 UI 연결
    }
}
