using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;

    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bulletPrebab;
    [SerializeField] private Transform armPivot;

    [SerializeField] private Camera mainCamera;

    void Start()
    {
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 20.0f;
        Vector3 difference = mainCamera.ScreenToWorldPoint(mousePosition) - armPivot.position;
        difference.z = 0.0f;

        difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        armPivot.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90.0f);

        if (rotationZ < -90 || rotationZ > 90)
        {
            if (transform.eulerAngles.y == 0)
            {
                armPivot.localRotation = Quaternion.Euler(180.0f, 0.0f, -rotationZ - 90.0f);
            }
            else if (transform.eulerAngles.y == 180)
            {
                armPivot.localRotation = Quaternion.Euler(180.0f, 180.0f, -rotationZ - 90.0f);
            }
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0.0f, 0.0f);
        transform.position += Time.deltaTime * movementSpeed * movementDirection;

        if (Input.GetButtonDown("Fire1"))
        {
            GameObject bullet = Instantiate(bulletPrebab, shootPoint.position, Quaternion.LookRotation(armPivot.forward, armPivot.up));
            bullet.transform.rotation = Quaternion.Euler(0.0f, bullet.transform.eulerAngles.y, bullet.transform.eulerAngles.z - 90.0f);
        }
    }
}
