using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHp = 100;

    int currentHp;

    void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        Debug.Log(gameObject.name + " HP : " + currentHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " Dead");

        gameObject.SetActive(false);
    }
}