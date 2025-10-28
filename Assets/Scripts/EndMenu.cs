using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    // AUDIO
    [SerializeField] AudioClip clickSound;

    private System.Collections.IEnumerator PlaySoundAndLoadScene()
    {
        if (clickSound)
        {
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);

            // Wait for the exact duration of the audio clip
            yield return new WaitForSeconds(clickSound.length);
        }

        SceneManager.LoadScene("MainMenu");
    }

    private System.Collections.IEnumerator PlaySoundAndQuit()
    {
        if (clickSound)
        {
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);

            // Wait for the exact duration of the audio clip
            yield return new WaitForSeconds(clickSound.length);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    public void LoadMenu()
    {
        StartCoroutine(PlaySoundAndLoadScene());
    }

    public void QuitGame()
    {
        StartCoroutine(PlaySoundAndQuit());
    }
}