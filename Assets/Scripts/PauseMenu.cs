using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;

    [Header("First Selected Buttons")]
    [SerializeField] private GameObject pauseFirstButton;
    [SerializeField] private GameObject gameOverFirstButton;
    [SerializeField] private GameObject winFirstButton;

    private void Awake()
    {
        IsPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if ((gameOverPanel != null && gameOverPanel.activeSelf) ||
                (winPanel != null && winPanel.activeSelf))
                return;

            if (IsPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;
        if (pausePanel != null) pausePanel.SetActive(true);
        SelectButton(pauseFirstButton);
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0f;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        SelectButton(gameOverFirstButton);
    }

    public void ShowWinScreen()
    {
        Time.timeScale = 0f;
        if (winPanel != null) winPanel.SetActive(true);
        SelectButton(winFirstButton);
    }

    public void Restart()
    {
        if (GameManager.currentLives <= 0)
        {
            GameManager.ResetLives();
        }
        Resume();

        if (LevelLoader.Instance != null) LevelLoader.Instance.ReloadCurrent();
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Resume();

        if (LevelLoader.Instance != null) LevelLoader.Instance.LoadNextLevel();
        else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void QuitToMenu()
    {
        GameManager.ResetLives();
        Resume();

        if (LevelLoader.Instance != null) LevelLoader.Instance.LoadMainMenu();
        else SceneManager.LoadScene(0);
    }

    private void SelectButton(GameObject button)
    {
        if (EventSystem.current != null && button != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(button);
        }
    }
}