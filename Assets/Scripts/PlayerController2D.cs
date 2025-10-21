using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Swim Movement")]
    [SerializeField] float baseSpeed = 6f;        // normal
    [SerializeField] float sprintMult = 1.5f;     // Left Shift
    [SerializeField] float crouchMult = 0.6f;     // Left Ctrl
    [SerializeField] float sinkForce = 2f;        // constant downward pull
    [SerializeField] float swimUpForce = 18f;     // W/Up to hold position or rise
    [SerializeField] float swimDownForce = 20f;   // S key for faster descent
    [SerializeField] float momentumDecay = 1.5f;
    [SerializeField] float maxSpeed = 7f;

    [Header("Dash (backwards)")]
    [SerializeField] float dashImpulse = 10f;     // Space: burst opposite facing
    [SerializeField] float dashCooldown = 0.35f;
    float lastDashTime = -999f;

    [Header("Grounding (for slopes/edges if needed)")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.15f;
    [SerializeField] LayerMask groundMask;

    [Header("Attack")]
    [SerializeField] Collider2D attackZone;       // small child trigger
    [SerializeField] float attackTime = 0.15f;

    Rigidbody2D rb;
    SpriteRenderer sr;
    float inputX, inputY;
    bool swimUpHeld, swimDownHeld, dashPressed, attacking;
    Vector2 lastMoveDir = Vector2.right;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Input (old system; works if Active Input Handling = Both)
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        swimUpHeld = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        swimDownHeld = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        if (Input.GetKeyDown(KeyCode.Space)) dashPressed = true;
        if (Input.GetMouseButtonDown(0) && !attacking) StartCoroutine(DoAttack());

        // Facing direction based on last movement
        if (Mathf.Abs(inputX) > 0.01f) {
            lastMoveDir = new Vector2(Mathf.Sign(inputX), 0);
            // If your shrimp art faces LEFT by default, flip when moving RIGHT:
            sr.flipX = inputX > 0;   // swap to false if art faces RIGHT by default
        } else if (Mathf.Abs(inputY) > 0.01f) {
            lastMoveDir = new Vector2(0, Mathf.Sign(inputY));
        }

        // Vertical aiming rotation (for up/down swimming)
        // Tilt only when using arrow keys
        bool grounded = IsGrounded();
        if (!grounded)
        {
            if (Input.GetKey(KeyCode.UpArrow))
                transform.rotation = sr.flipX ? Quaternion.Euler(0, 0, 30) : Quaternion.Euler(0, 0, -30); // face up
            else if (Input.GetKey(KeyCode.DownArrow))
                transform.rotation = sr.flipX ? Quaternion.Euler(0, 0, -60) : Quaternion.Euler(0, 0, 60); // face down
            else
                transform.rotation = Quaternion.Euler(0, 0, 0); // face sideways normally
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void FixedUpdate()
    {
        // Horizontal swim
        Vector2 v = rb.linearVelocity;
        float speed = baseSpeed;

        // Speed modifiers
        if (Input.GetKey(KeyCode.LeftShift)) speed *= sprintMult;
        if (Input.GetKey(KeyCode.LeftControl)) speed *= crouchMult;

        // Momentum: keep some horizontal velocity after key release
        if (Mathf.Abs(inputX) > 0.01f) v.x = Mathf.Lerp(v.x, inputX * speed, Time.fixedDeltaTime * 5f);
        else v.x = Mathf.Lerp(v.x, 0, Time.fixedDeltaTime * momentumDecay);

        //v.x = inputX * speed; // cancels out momentum Lerp and removing it restores the glide

        // Vertical: constant sink + hold W to push up
        // Floaty physics
        v += Vector2.down * sinkForce * Time.fixedDeltaTime;
        // Up movement (only if moving horizontally)
        if (swimUpHeld && Mathf.Abs(inputX) > 0.1f)
            v += Vector2.up * swimUpForce * Time.fixedDeltaTime;
        else if (swimUpHeld)
            v += Vector2.up * (swimUpForce * 0.9f) * Time.fixedDeltaTime; // slower vertical lift if stationary / only slow when not moving horizontally
        // Down movement (S key for faster sinking)
        if (swimDownHeld)
            v += Vector2.down * swimDownForce * Time.fixedDeltaTime;

        // Clamp max speed
        v.x = Mathf.Clamp(v.x, -maxSpeed, maxSpeed);
        v.y = Mathf.Clamp(v.y, -maxSpeed, maxSpeed);
        rb.linearVelocity = v;

        // Backward dash (impulse opposite facing)
        // Dash — disable normal control briefly and only move opposite of facing
        if (dashPressed && Time.time - lastDashTime >= dashCooldown)
        {
            StartCoroutine(DashCoroutine());
            lastDashTime = Time.time;
        }
        dashPressed = false;
    }

    bool IsGrounded()
    {
        if (!groundCheck) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
    }

    System.Collections.IEnumerator DoAttack()
    {
        attacking = true;
        if (attackZone) attackZone.enabled = true;
        yield return new WaitForSeconds(attackTime);
        if (attackZone) attackZone.enabled = false;
        attacking = false;
    }

    System.Collections.IEnumerator DashCoroutine()
    {
        float dashTime = 0.2f;
        rb.linearVelocity = Vector2.zero;
        float facing = sr.flipX ? 1f : -1f; // flipX true = facing right visually
        Vector2 dir = new Vector2(facing, 0f) * -1f;  // tail direction / opposite facing
        rb.AddForce(dir * dashImpulse, ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashTime);
        rb.linearVelocity = Vector2.zero; // pause briefly
    }

    void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}