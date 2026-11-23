using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }

    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioMixerGroup musicMixerGroup;

    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = GetComponent<AudioSource>();
        musicSource.clip = backgroundMusic;
        musicSource.outputAudioMixerGroup = musicMixerGroup;
        musicSource.loop = true;
        musicSource.spatialBlend = 0f; // 2D non-spatialized music
        musicSource.playOnAwake = true;

        if (backgroundMusic != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }
}