using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Level Select Buttons (Order: Level 1 to Level 5)")]
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private TMP_Text[] levelLabels;

    [Header("Default Keyboard Focus")]
    [SerializeField] private GameObject firstSelectedButton;

    private void Start()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        GameManager.ResetLives();

        RefreshLevelButtons();

        if (EventSystem.current != null && firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    public void RefreshLevelButtons()
    {
        int unlocked = SaveData.GetUnlockedLevel();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1;
            bool isUnlocked = levelIndex <= unlocked;

            if (levelButtons[i] != null)
            {
                levelButtons[i].interactable = isUnlocked;
            }

            if (i < levelLabels.Length && levelLabels[i] != null)
            {
                if (!isUnlocked)
                {
                    levelLabels[i].text = $"Level {levelIndex}\nLocked";
                }
                else
                {
                    float best = SaveData.GetBestTime(levelIndex);
                    string bestStr = best > 0f ? $"Best: {best:0.0}s" : "Best: --";
                    levelLabels[i].text = $"Level {levelIndex}\n{bestStr}";
                }
            }
        }
    }

    public void PlayLatestUnlocked()
    {
        int target = Mathf.Clamp(SaveData.GetUnlockedLevel(), 1, 5);
        LoadLevel(target);
    }

    public void LoadLevel(int buildIndex)
    {
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.PlayButton();
        SceneManager.LoadScene(buildIndex);
    }

    public void OpenCredits()
    {
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.PlayButton();
        SceneManager.LoadScene(6);
    }

    public void OnResetProgress()
    {
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.PlayButton();
        SaveData.ResetProgress();
        RefreshLevelButtons();
    }

    public void QuitGame()
    {
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.PlayButton();
        Application.Quit();
    }
}