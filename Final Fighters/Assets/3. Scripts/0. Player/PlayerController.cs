using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Number")]
    public int playerNumber = 1;

    [Header("Move")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    Rigidbody2D rb;
    PlayerCombat combat;

    float moveInput;

    bool isGrounded;
    bool isDashing;

    float dashTimer;
    float dashCooldownTimer;

    int facing = 1;

    KeyCode leftKey;
    KeyCode rightKey;
    KeyCode jumpKey;
    KeyCode dashKey;
    KeyCode attackKey;
    KeyCode guardKey;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        combat = GetComponent<PlayerCombat>();

        SetKeys();
    }

    void Update()
    {
        GroundCheck();

        HandleInput();
        HandleDash();
        Flip();
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    void SetKeys()
    {
        // PLAYER 1
        if (playerNumber == 1)
        {
            leftKey = KeyCode.A;
            rightKey = KeyCode.D;
            jumpKey = KeyCode.W;

            dashKey = KeyCode.LeftControl;

            attackKey = KeyCode.Space;
            guardKey = KeyCode.LeftShift;
        }

        // PLAYER 2
        else if (playerNumber == 2)
        {
            leftKey = KeyCode.LeftArrow;
            rightKey = KeyCode.RightArrow;
            jumpKey = KeyCode.UpArrow;

            dashKey = KeyCode.RightControl;

            attackKey = KeyCode.Keypad0;
            guardKey = KeyCode.Keypad1;
        }
    }

    void HandleInput()
    {
        moveInput = 0;

        if (Input.GetKey(leftKey))
        {
            moveInput = -1;
        }

        if (Input.GetKey(rightKey))
        {
            moveInput = 1;
        }

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetKeyDown(dashKey) && dashCooldownTimer <= 0)
        {
            StartDash();
        }

        if (Input.GetKeyDown(attackKey))
        {
            combat.Attack();
        }

        combat.isGuarding = Input.GetKey(guardKey);
    }

    void StartDash()
    {
        isDashing = true;

        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        rb.velocity = new Vector2(facing * dashSpeed, 0);
    }

    void HandleDash()
    {
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0)
            {
                isDashing = false;
            }
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer
        );
    }

    void Flip()
    {
        if (moveInput > 0)
        {
            facing = 1;
        }
        else if (moveInput < 0)
        {
            facing = -1;
        }

        transform.localScale = new Vector3(facing, 1, 1);
    }

    public int GetFacing()
    {
        return facing;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}