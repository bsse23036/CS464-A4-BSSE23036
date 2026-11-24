using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    [SerializeField] private GameObject backButton;

    private void Start()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(SelectBackNextFrame());
    }

    private void Update()
    {
        if (EventSystem.current != null &&
            EventSystem.current.currentSelectedGameObject == null &&
            backButton != null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                EventSystem.current.SetSelectedGameObject(backButton);
            }
        }
    }

    private IEnumerator SelectBackNextFrame()
    {
        yield return null;
        if (EventSystem.current != null && backButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(backButton);
        }
    }

    public void BackToMenu()
    {
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.PlayButton();

        if (LevelLoader.Instance != null) LevelLoader.Instance.LoadMainMenu();
        else SceneManager.LoadScene(0);
    }
}