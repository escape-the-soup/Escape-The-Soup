using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance { get; private set; }

    // AUDIO
    [SerializeField] AudioClip winOpenSound;

    [Header("Collectibles / Exit")]
    public GameObject waferGroup;   // parent holding all wafers (set inactive at start)
    public int wafersNeeded = 3;
    public GameObject exitTrigger;  // collider object, set inactive at start

    int collected = 0;

    void Awake() { Instance = this; }

    public void EnableWafers()
    {
        collected = 0;
        if (waferGroup) waferGroup.SetActive(true);
    }

    public void OnWaferCollected()
    {
        collected++;
        if (collected >= wafersNeeded && exitTrigger)
        {
            exitTrigger.SetActive(true);
            if (winOpenSound) AudioSource.PlayClipAtPoint(winOpenSound, Camera.main.transform.position, 0.7f);
        }
        
    }
}