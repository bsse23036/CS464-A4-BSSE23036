using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    private bool isStopped;

    public float ElapsedTime => Time.timeSinceLevelLoad;

    private void Update()
    {
        if (isStopped || PauseMenu.IsPaused || timerText == null) return;

        float t = Time.timeSinceLevelLoad;
        int minutes = Mathf.FloorToInt(t / 60f);
        float seconds = t % 60f;
        timerText.text = $"{minutes:00}:{seconds:00.0}";
    }

    public void StopTimer()
    {
        isStopped = true;
    }
}