using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD Components")]
    public HealthBar healthBar;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI livesText;

    private Health playerHealth;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.Damaged += OnPlayerDamaged;
                OnPlayerDamaged(playerHealth.Current, playerHealth.maxHealth);
            }
        }
    }

    void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.Damaged -= OnPlayerDamaged;
        }
    }

    private void OnPlayerDamaged(int current, int max)
    {
        if (healthBar != null)
        {
            healthBar.Set(current, max);
        }
        UpdateHealthDisplay(current);
    }

    public void UpdateCoinsDisplay(int coinCount)
    {
        if (coinsText != null) coinsText.text = "Coins: " + coinCount;
    }

    public void UpdateHealthDisplay(int currentHealth)
    {
        if (healthText != null)
        {
            string heartsDisplay = "";
            for (int i = 0; i < currentHealth; i++)
            {
                heartsDisplay += "♥ ";
            }
            healthText.text = "Health: " + heartsDisplay.TrimEnd();
        }
    }

    public void UpdateTimerDisplay(float timeRemaining)
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            timerText.text = "Time: " + seconds + "s";
        }
    }

    public void UpdateLivesDisplay(int lives)
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + Mathf.Max(0, lives);
        }
    }
}