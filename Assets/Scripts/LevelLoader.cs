using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 0.35f;

    private bool isTransitioning;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }

    public void LoadSceneByIndex(int buildIndex)
    {
        if (isTransitioning) return;
        StartCoroutine(TransitionRoutine(buildIndex));
    }

    public void ReloadCurrent()
    {
        LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
        {
            LoadSceneByIndex(next);
        }
        else
        {
            LoadCredits();
        }
    }

    public void LoadMainMenu() => LoadSceneByIndex(0);
    public void LoadCredits() => LoadSceneByIndex(6);

    private IEnumerator TransitionRoutine(int targetBuildIndex)
    {
        isTransitioning = true;
        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.blocksRaycasts = true;
            yield return Fade(0f, 1f);
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(targetBuildIndex);
        while (op != null && !op.isDone)
        {
            yield return null;
        }

        if (fadeCanvasGroup != null)
        {
            yield return Fade(1f, 0f);
            fadeCanvasGroup.blocksRaycasts = false;
        }

        isTransitioning = false;
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = to;
    }
}