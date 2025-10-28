using UnityEngine;

public class HandHazard : MonoBehaviour
{
    public Transform strikePosition; // where hand goes down (child target at bottom)
    public float strikeSpeed = 10f;
    //public float strikeDelay = 1f; // one-second warning
    public float resetDelay = 1.5f;
    public bool strikeOnlyOnce = true;

    [SerializeField] float damageRadius = 3f; // radius of area where shrimp can take damage
    [SerializeField] Vector2 damageOffset = new Vector2(0f, -3f); // moves above area down

    // AUDIO
    [SerializeField] AudioClip strikeSound;
    [SerializeField] AudioClip resetSound;

    Vector3 startPosition;
    Vector3 strikeWorldPos; // initial world position so that StrikePosition can stay as a child of HandHazard but it stays in place instead of respective to it
    bool striking = false;
    bool hasStruck = false;

    void Start()
    {
        startPosition = transform.position;
        strikeWorldPos = strikePosition.position; // store world position once
    }

    void Update()
    {
        if (!striking) return;
        
        // move down towards strike
        transform.position = Vector3.MoveTowards(transform.position, strikeWorldPos, strikeSpeed * Time.deltaTime);
        
        // reached bottom (strike point)
        if (Vector3.SqrMagnitude(transform.position - strikeWorldPos) < 0.15f) // Vector3.Distance(transform.position, strikePosition.position) < 0.05f
        {

            // stop movement
            transform.position = strikeWorldPos;
            striking = false; // stop moving and lock position

            // Deal damage NOW if the player is still under the hand
            Collider2D player = Physics2D.OverlapCircle((Vector2)transform.position + damageOffset, damageRadius, LayerMask.GetMask("Player"));
            if (player)
            {
                // Damage player instantly
                PlayerHealth ph = player.GetComponent<PlayerHealth>();
                if (ph) ph.TakeDamage(100); // instant kill
            }

            // Reset after short delay
            Invoke(nameof(ResetPositionAndEnableWafers), resetDelay);
        }
    }

    void ResetPositionAndEnableWafers()
    {
        if (resetSound) AudioSource.PlayClipAtPoint(resetSound, transform.position);
        transform.position = startPosition;
        hasStruck = false; // reset after going back up
        GameFlow.Instance.EnableWafers();   // wafers appear after the first strike
    }

    public void TriggerStrike()
    {
        if (!hasStruck)
        {
            if (strikeSound) AudioSource.PlayClipAtPoint(strikeSound, transform.position);
            hasStruck = true;
            float strikeDelay = Random.Range(0.4f, 1.5f);
            Invoke(nameof(StartStrike), strikeDelay);
        }
    }

    void StartStrike()
    {
        striking = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Damage player instantly
        //PlayerHealth ph = other.GetComponent<PlayerHealth>();
        //if (ph) ph.TakeDamage(100); // instant kill

        // Start strike motion only the first time
        if (!hasStruck || !strikeOnlyOnce)
        {
            TriggerStrike();
        }
    }

    void OnDrawGizmosSelected()
    {
        // where shrimp will take damage
        Gizmos.color = Color.red;
        Vector3 offsetPos = transform.position + (Vector3)damageOffset;
        Gizmos.DrawWireSphere(offsetPos, damageRadius);
    }
}