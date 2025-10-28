using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    // public static ExitTrigger instance; is essentially handled by GameFlow
    public string nextSceneName = "WinScene";

    // void Awake() => instance = this; is essentially handled by GameFlow

    // public void ActivateExit() => gameObject.SetActive(true); is essentially handled by GameFlow

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            SceneManager.LoadScene(nextSceneName);
    }
}