using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class ListenerOnPlayer : MonoBehaviour
{
    private AudioListener playerListener;

    private void Awake()
    {
        playerListener = GetComponent<AudioListener>();
        playerListener.enabled = true;

        // Disable any other active AudioListeners (e.g., on Main Camera)
        AudioListener[] allListeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        foreach (AudioListener listener in allListeners)
        {
            if (listener != playerListener)
            {
                listener.enabled = false;
            }
        }
    }
}