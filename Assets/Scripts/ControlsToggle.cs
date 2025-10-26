using UnityEngine;
using TMPro;

public class ControlsToggle : MonoBehaviour
{
    TextMeshProUGUI controlsText;

    void Awake() => controlsText = GetComponent<TextMeshProUGUI>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            controlsText.enabled = !controlsText.enabled;
    }
}