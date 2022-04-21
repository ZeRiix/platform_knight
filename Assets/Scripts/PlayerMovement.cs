using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float climbSpeed;
    public float jumpForce;

    private bool isJumping;
    private bool isGrounded;
    [HideInInspector]
    public bool isClimbing;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask collisionLayers;

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public CapsuleCollider2D playerCollider;

    private Vector3 velocity = Vector3.zero;
    private float horizontalMovement;
    private float verticalMovement;

    public static PlayerMovement instance;
    public bool jp;
    public bool lf;
    public bool ri;

    public Button right;
    public Button left;
    public Button jump;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerMovement dans la scène");
            return;
        }

        instance = this;
    }

    void Update()
    {
        /* version qui beug malheuresement
        Button btn = right.GetComponent<Button>();
        btn.onClick.AddListener(() => {
            rightMouv();
        });
        Button btn2 = left.GetComponent<Button>();
        btn2.onClick.AddListener(() => {
            leftMouv();
        });
        Button btn3 = jump.GetComponent<Button>();
        btn3.onClick.AddListener(() => {
            jumpMouv();
        });*/

        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;
        verticalMovement = Input.GetAxis("Vertical") * climbSpeed * Time.fixedDeltaTime;
        Debug.Log(verticalMovement);
        Debug.Log(horizontalMovement);

        if (lf == true && ri == false)
        {
            horizontalMovement = -4;
        }
        if (ri == true && lf == false)
        {
            horizontalMovement = 4;
        }
        if (lf == false && ri == false)
        {
            horizontalMovement = 0;
        }

        if (Input.GetButtonDown("Jump") || jp == true && isGrounded && !isClimbing)
        {
            isJumping = true;
        }

        Flip(rb.velocity.x);

        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("Speed", characterVelocity);
        animator.SetBool("isClimbing", isClimbing);
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);
        MovePlayer(horizontalMovement, verticalMovement);
    }

    void MovePlayer(float _horizontalMovement, float _verticalMovement)
    {
        if (!isClimbing)
        {
            Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);

            if (isJumping)
            {
                rb.AddForce(new Vector2(0f, jumpForce));
                isJumping = false;
            }
        }
        else
        {
            Vector3 targetVelocity = new Vector2(0, _verticalMovement);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
        }

    }

    void Flip(float _velocity)
    {
        if (_velocity > 0.1f)
        {
            spriteRenderer.flipX = false;
        }else if(_velocity < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void leftMouv()
    {
        ri = false;
        if (lf == true) {
            lf = false;
        }
        else if (lf == false)
        {
            lf = true;
        }
    }

    public void rightMouv()
    {
        lf = false;
        if (ri == true) {
            ri = false;
        }
        else if (ri == false)
        {
            ri = true;
        }
    }

    public void jumpMouv()
    {
        if (jp == true ) {
            jp = false;
        }
        else if (jp == false)
        {
            jp = true;
        }
    }
}
