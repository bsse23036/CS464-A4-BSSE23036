using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
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

    private GameObject currentDefaultButton;

    private void Awake()
    {
        IsPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);

        EnsureEventSystemExists();
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

        // Restore keyboard focus if clicking empty space cleared selection while a menu is open
        if (IsPaused && EventSystem.current != null &&
            EventSystem.current.currentSelectedGameObject == null &&
            currentDefaultButton != null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
                Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
                Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                EventSystem.current.SetSelectedGameObject(currentDefaultButton);
            }
        }
    }

    public void Pause()
    {
        OpenMenuState(pausePanel, pauseFirstButton, pauseAudio: true);
    }

    public void Resume()
    {
        ResetMenuState();
        LockCursor();
    }

    private void ResetMenuState()
    {
        IsPaused = false;
        currentDefaultButton = null;
        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        OpenMenuState(gameOverPanel, gameOverFirstButton, pauseAudio: false);
    }

    public void ShowWinScreen()
    {
        OpenMenuState(winPanel, winFirstButton, pauseAudio: false);
    }

    private void OpenMenuState(GameObject panel, GameObject firstButton, bool pauseAudio)
    {
        IsPaused = true;
        currentDefaultButton = firstButton;
        Time.timeScale = 0f;
        if (pauseAudio) AudioListener.pause = true;

        UnlockCursor();

        if (panel != null) panel.SetActive(true);
        StartCoroutine(SelectButtonNextFrame(firstButton));
    }

    private IEnumerator SelectButtonNextFrame(GameObject button)
    {
        yield return null; // Wait 1 unscaled frame so newly activated UI Buttons register

        EnsureEventSystemExists();
        if (EventSystem.current != null && button != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(button);
        }
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void EnsureEventSystemExists()
    {
        if (EventSystem.current == null && FindFirstObjectByType<EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<InputSystemUIInputModule>();
        }
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
        ResetMenuState();
        UnlockCursor();

        if (LevelLoader.Instance != null) LevelLoader.Instance.LoadMainMenu();
        else SceneManager.LoadScene(0);
    }
}