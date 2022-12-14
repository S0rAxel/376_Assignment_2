using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;

    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bulletPrebab;
    [SerializeField] private Transform armPivot;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private float mJumpForce;
    [SerializeField] private LayerMask mWhatIsGround;

    [SerializeField] private int healthPoints = 3;

    private float kGroundCheckRadius = 0.1f;
    private float kDamagePushForce = 5.0f;
    private float kInvincibilityDuration = 1.0f;
    private float mInvincibleTimer;

    // Animator booleans
    private bool mRunning;
    private bool mGrounded;
    private bool mRising;
    private bool mInvincible;


    // Wall kicking
    private bool mAllowWallKick;
    private Vector2 mFacingDirection;

    // References to other components and game objects
    private Animator mAnimator;
    private Rigidbody2D mRigidBody2D;
    private GroundCheck mGroundCheck;

    // Reference to audio sources
    private AudioSource mWallKickSound;
    private AudioSource mTakeDamageSound;

    [SerializeField] private GameObject mDeathParticleEmitter;
    //[SerializeField] private LifeMeter life;

    private SpriteRenderer[] sprites;

    void Start()
    {
        // Get references to other components and game objects
        mAnimator = GetComponent<Animator>();
        mRigidBody2D = GetComponent<Rigidbody2D>();
        mGroundCheck = GetComponentInChildren<GroundCheck>();

        sprites = GetComponentsInChildren<SpriteRenderer>();

        //// Get audio references
        //AudioSource[] audioSources = GetComponents<AudioSource>();
        //mLandingSound = audioSources[0];
        //mWallKickSound = audioSources[1];
        //mTakeDamageSound = audioSources[2];

        mFacingDirection = Vector2.right;
    }

    public void TakeDamage(int damage)
    {
        if (!mInvincible)
        {

            foreach (SpriteRenderer sRenderer in sprites)
            {
                sRenderer.DOColor(Color.red, 0.1f).SetLoops(4, LoopType.Yoyo);
            }

            Vector2 forceDirection = new Vector2(-mFacingDirection.x, 1.0f) * kDamagePushForce;
            mRigidBody2D.velocity = Vector2.zero;
            mRigidBody2D.AddForce(forceDirection, ForceMode2D.Impulse);
            mInvincible = true;
            //mTakeDamageSound.Play();

            healthPoints -= damage;

            //foreach (SpriteRenderer sRenderer in sprites) sRenderer.color = Color.white;
            if (healthPoints == 0)
            {
                Death();
            }
        }

    }

    private void Update()
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
                if (mFacingDirection == Vector2.right)
                {
                    armPivot.localRotation = Quaternion.Euler(180.0f, 0.0f, -rotationZ - 90.0f);
                }
                else
                {
                    armPivot.localRotation = Quaternion.Euler(0.0f, 180.0f, rotationZ - 90.0f);
                }
            }
        }

        //float horizontalInput = Input.GetAxis("Horizontal");

        //Vector3 movementDirection = new Vector3(horizontalInput, 0.0f, 0.0f);
        //transform.position += Time.deltaTime * movementSpeed * movementDirection;
        mRunning = false;
        if (Input.GetButton("Left"))
        {
            transform.Translate(-Vector2.right * movementSpeed * Time.deltaTime);
            FaceDirection(-Vector2.right);
            mRunning = true;
        }
        if (Input.GetButton("Right"))
        {
            transform.Translate(Vector2.right * movementSpeed * Time.deltaTime);
            FaceDirection(Vector2.right);
            mRunning = true;
        }


        if (Input.GetButtonDown("Fire1"))
        {
            GameObject bullet = Instantiate(bulletPrebab, shootPoint.position, Quaternion.LookRotation(armPivot.forward, armPivot.up));
            bullet.transform.rotation = Quaternion.Euler(0.0f, bullet.transform.eulerAngles.y, bullet.transform.eulerAngles.z - 90.0f);
            bullet.GetComponent<Bullet>().SetDirection(shootPoint.forward);
        }

        bool grounded = CheckGrounded();

        mGrounded = grounded;

        if (mGrounded && Input.GetButtonDown("Jump"))
        {
            mRigidBody2D.AddForce(Vector2.up * mJumpForce, ForceMode2D.Impulse);
        }
        else if (mAllowWallKick && Input.GetButtonDown("Jump"))
        {
            mRigidBody2D.velocity = Vector2.zero;
            mRigidBody2D.AddForce(Vector2.up * mJumpForce, ForceMode2D.Impulse);
            mWallKickSound.Play();
        }

        mRising = mRigidBody2D.velocity.y > 0.0f;
        UpdateAnimator();

        if (mInvincible)
        {
            mInvincibleTimer += Time.deltaTime;
            if (mInvincibleTimer >= kInvincibilityDuration)
            {
                mInvincible = false;
                mInvincibleTimer = 0.0f;
            }
        }
    }

    private bool CheckGrounded()
    {
        if (mGroundCheck.CheckGrounded(kGroundCheckRadius, mWhatIsGround, gameObject))
        {
            return true;
        }
        return false;
    }

    private void FaceDirection(Vector2 direction)
    {
        mFacingDirection = direction;
        if (direction == Vector2.right)
        {
            Vector3 newScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            transform.localScale = newScale;
        }
        else
        {
            Vector3 newScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            transform.localScale = newScale;
        }
    }

    private void Death()
    {

    }

    private void UpdateAnimator()
    {
        mAnimator.SetBool("bIsWalking", mRunning);
        mAnimator.SetBool("isGrounded", mGrounded);
        mAnimator.SetBool("isRising", mRising);
    }
}
