using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;  // assign the popup panel here
    public GameObject pauseButton; // the 3-bar icon
    bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
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
    }

    public void QuitGame()
    {
        Time.timeScale = 1; // unpause before quitting
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // stop play mode in editor
        #else
        Application.Quit(); // actually quit the built game
        #endif
    }
}