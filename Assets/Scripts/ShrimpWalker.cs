using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShrimpWalker : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    Rigidbody2D rb;
    SpriteRenderer sr;
    float inputX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal"); // A/D

        // Flip shrimp
        if (inputX > 0) sr.flipX = true;
        else if (inputX < 0) sr.flipX = false;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);
    }
}