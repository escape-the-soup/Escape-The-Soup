using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;
    public Slider healthBar;
    public string loseScene = "GameOver";

    // AUDIO
    [SerializeField] AudioClip deathSound;

    void Awake() // or Start() probably
    {
        currentHealth = maxHealth;
        if (healthBar) healthBar.maxValue = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        if (currentHealth < 0) currentHealth = 0; // in case a hit takes more health than the player has, they still die
        UpdateUI();
        if (currentHealth == 0) SceneManager.LoadScene(loseScene); ; // "death" occurs
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (healthBar) healthBar.value = currentHealth;
    }
}