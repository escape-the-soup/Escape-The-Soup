using UnityEngine;

public class AlgaeWafer : MonoBehaviour
{
    // public static int collected = 0; is handled by GameFlow
    public AudioClip collectSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // collected++; is handled by GameFlow
        GameFlow.Instance.OnWaferCollected();
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
        Destroy(gameObject);

        // if (collected >= 3) ExitTrigger.instance.ActivateExit(); is essentially handled by GameFlow
    }
}