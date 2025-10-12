using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float jumpForce = 14f;

    [Header("Grounding")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.15f;
    [SerializeField] LayerMask groundMask;

    Rigidbody2D rb;
    float inputX;
    bool jumpPressed;

    void Awake() { rb = GetComponent<Rigidbody2D>(); }

    void Update()
    {
        // WASD / Arrow keys (uses old Input Manager)
        inputX = Input.GetAxisRaw("Horizontal");          // A/D or Left/Right
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            jumpPressed = true;

        // Optional: face direction (if you add a SpriteRenderer later)
        // if (inputX != 0) transform.localScale = new Vector3(Mathf.Sign(inputX), 1, 1);

        Debug.Log(inputX);
    }

    void FixedUpdate()
    {
        // Horizontal
        rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);

        // Jump
        if (jumpPressed && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // cancel downward vel for consistent jump
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        jumpPressed = false;
    }

    bool IsGrounded()
    {
        if (!groundCheck) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
    }

    // Visualize ground check in editor
    void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}