using UnityEngine;
using TMPro;

public class ControlsToggle : MonoBehaviour
{
    TextMeshProUGUI controlsText;

    // AUDIO
    [SerializeField] AudioClip clickSound;

    void Awake() => controlsText = GetComponent<TextMeshProUGUI>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (clickSound) AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
            controlsText.enabled = !controlsText.enabled;
        }
    }
}