using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float timerToDestroy;

    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        rigidbody.velocity = transform.forward * speed;
    }

    private void Start()
    {
        Destroy(gameObject, timerToDestroy);
    }

    public void SetDirection(Vector2 direction)
    {
        rigidbody.velocity = direction * speed;
    }

}
