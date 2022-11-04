using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float timeToRevive;
    [SerializeField] protected float timeToDisable;
    [SerializeField] protected float movementSpeedTime;
    [SerializeField] protected int health;

    protected int maxHealth;

    [SerializeField] protected Transform[] waypoints;

    protected Animator animator;
    protected BoxCollider2D boxCollider;
    protected SpriteRenderer sprite;

    protected int waypointIndex = 0;
    protected bool isDead;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        maxHealth = health;
    }

    protected virtual void Start()
    {
        if (waypoints.Length > 0)
        {
            Move();
        }
    }

    private void Update()
    {
    }

    public virtual void TakeDamage(int damage)
    {
        if (!isDead)
        {
            health--;
            if (health <= 0)
            {
                Death();
            } 
        }
    }

    protected virtual void Move()
    {

    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            if (collision.GetComponent<Bullet>().BulleLife == 1)
            {
                ScoreManager.Instance.IncreaseScore(5);
            }
        }

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(1);
        }
    }

    public void Spawn()
    {
        health = maxHealth;
        boxCollider.enabled = true;
        isDead = false;
    }

    protected virtual void Death()
    {
        isDead = true;
        animator.SetBool("isDead", isDead);
        boxCollider.enabled = false;
    }
}
