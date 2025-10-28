using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;  // assign the popup panel here
    public GameObject pauseButton; // the 3-bar icon
    bool isPaused = false;

    // AUDIO
    [SerializeField] AudioClip clickSound;
    [SerializeField] AudioClip resumeSound;
    [SerializeField] AudioClip quitSound;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        if (clickSound) AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
        isPaused = !isPaused;
        pauseUI.SetActive(isPaused);
        pauseButton.SetActive(!isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseUI.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
        if (resumeSound) AudioSource.PlayClipAtPoint(resumeSound, Camera.main.transform.position);
    }

    public void QuitGame()
    {
        StartCoroutine(PlaySoundAndQuit());
        
    }

    private System.Collections.IEnumerator PlaySoundAndQuit()
    {
        Time.timeScale = 1; // unpause before quitting

        if (quitSound)
        {
            AudioSource.PlayClipAtPoint(quitSound, Camera.main.transform.position);

            // Wait for the exact duration of the audio clip
            yield return new WaitForSeconds(quitSound.length);
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // stop play mode in editor
#else
        Application.Quit(); // actually quit the built game
#endif
    }
}