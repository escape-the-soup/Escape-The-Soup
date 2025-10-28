using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // AUDIO
    [SerializeField] AudioClip playSound;
    [SerializeField] AudioClip quitSound;

    private System.Collections.IEnumerator PlaySoundAndLoadScene()
    {
        if (playSound)
        {
            AudioSource.PlayClipAtPoint(playSound, Camera.main.transform.position);

            // Wait for the exact duration of the audio clip
            yield return new WaitForSeconds(playSound.length);
        }

        SceneManager.LoadScene("Main");
    }

    private System.Collections.IEnumerator PlaySoundAndQuit()
    {
        if (quitSound)
        {
            AudioSource.PlayClipAtPoint(quitSound, Camera.main.transform.position);

            // Wait for the exact duration of the audio clip
            yield return new WaitForSeconds(quitSound.length);
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void PlayGame() => StartCoroutine(PlaySoundAndLoadScene());
    public void QuitGame() => StartCoroutine(PlaySoundAndQuit());
}