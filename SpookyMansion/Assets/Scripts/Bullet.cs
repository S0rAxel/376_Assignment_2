using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float timerToDestroy;

    private int bulleLife = 2;

    private Rigidbody2D rigidbody;

    public int BulleLife { get => bulleLife; }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        rigidbody.velocity = transform.forward * speed;
    }

    private void Start()
    {
        Destroy(gameObject, timerToDestroy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(1);
            bulleLife--;

            if (bulleLife == 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetDirection(Vector2 direction)
    {
        rigidbody.velocity = direction * speed;
    }

}
