using UnityEngine;
using UnityEngine.Audio;

public class SfxPlayer : MonoBehaviour
{
    public static SfxPlayer Instance { get; private set; }

    [Header("Mixer Routing")]
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private int poolSize = 4;

    [Header("Sound Clips")]
    public AudioClip pickupClip;
    public AudioClip hitClip;
    public AudioClip buttonClip;
    public AudioClip winClip;
    public AudioClip gameOverClip;

    private AudioSource[] sources;
    private int nextIndex;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        sources = new AudioSource[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.spatialBlend = 0f;
            src.outputAudioMixerGroup = sfxMixerGroup;
            sources[i] = src;
        }
    }

    public void Play(AudioClip clip)
    {
        if (clip == null || sources == null || sources.Length == 0) return;

        AudioSource src = sources[nextIndex];
        nextIndex = (nextIndex + 1) % sources.Length;

        float semitones = Random.Range(-2f, 2f);
        src.pitch = Mathf.Pow(2f, semitones / 12f);
        src.PlayOneShot(clip);
    }

    public void PlayButton() => Play(buttonClip);
    public void PlayPickup() => Play(pickupClip);
    public void PlayHit() => Play(hitClip);
    public void PlayWin() => Play(winClip);
    public void PlayGameOver() => Play(gameOverClip);
}