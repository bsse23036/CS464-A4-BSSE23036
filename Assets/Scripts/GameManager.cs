using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Score & Level Settings")]
    public int startingLives = 3;
    public int coinsCollected = 0;
    public bool useTimer = true;
    public float levelTimer = 60f;

    [Header("Runtime Status")]
    [SerializeField] private float currentTimer;

    public static int currentLives = -1;
    private bool isLevelEnded = false;

    void Awake()
    {
        Instance = this;
        currentTimer = levelTimer;

        if (currentLives < 0)
        {
            currentLives = startingLives;
        }
    }

    void Start()
    {
        ResetLevelState();
    }

    public static void ResetLives(int defaultLives = 3)
    {
        currentLives = defaultLives;
    }

    private void ResetLevelState()
    {
        coinsCollected = 0;
        currentTimer = levelTimer;
        isLevelEnded = false;
        UpdateUI();
    }

    void Update()
    {
        if (!useTimer || isLevelEnded || PauseMenu.IsPaused) return;

        currentTimer -= Time.deltaTime;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateTimerDisplay(currentTimer);
        }

        if (currentTimer <= 0f)
        {
            currentTimer = 0f;
            PlayerDied();
        }
    }

    public void AddCoin()
    {
        coinsCollected++;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateCoinsDisplay(coinsCollected);
            UIManager.Instance.UpdateTimerDisplay(currentTimer);
            UIManager.Instance.UpdateLivesDisplay(currentLives);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Health playerHealth = player.GetComponent<Health>();
                if (playerHealth != null)
                {
                    if (UIManager.Instance.healthBar != null)
                        UIManager.Instance.healthBar.Set(playerHealth.Current, playerHealth.maxHealth);
                    UIManager.Instance.UpdateHealthDisplay(playerHealth.Current);
                }
            }
        }
    }

    public void PlayerDied()
    {
        if (isLevelEnded) return;
        isLevelEnded = true;

        currentLives--;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateLivesDisplay(currentLives);
        }

        if (currentLives > 0)
        {
            // Still have lives left: reload the current level automatically
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            // Out of lives: show the Game Over screen
            currentLives = 0;
            PauseMenu pauseMenu = FindFirstObjectByType<PauseMenu>();
            if (pauseMenu != null)
            {
                pauseMenu.ShowGameOver();
            }
        }
    }

    public void ReloadCurrentLevel()
    {
        PlayerDied();
    }

    public void LevelWon()
    {
        if (isLevelEnded) return;
        isLevelEnded = true;

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        float clearTime = Mathf.Max(0.1f, levelTimer - currentTimer);

        SaveData.RecordBestTime(currentIndex, clearTime);
        if (currentIndex >= 1 && currentIndex < 5)
        {
            SaveData.UnlockLevel(currentIndex + 1);
        }

        if (SfxPlayer.Instance != null)
        {
            SfxPlayer.Instance.PlayWin();
        }

        PauseMenu pauseMenu = FindFirstObjectByType<PauseMenu>();
        if (pauseMenu != null)
        {
            pauseMenu.ShowWinScreen();
        }
    }
}