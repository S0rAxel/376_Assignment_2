using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int points;

    protected void TakeDamage(int damage)
    {
        health--;
        if (health <= 0)
        {
            Death();
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    public void Spawn()
    {

    }
    protected void Death()
    {
        print("Enemy is Dead");
    }
}
