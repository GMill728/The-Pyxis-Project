using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip[] musicTracks;
    public AudioClip[] sfxClips;

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                PlayMusic(0);
                break;

            case "ProcGen Test":
                PlayMusic(1);
                break;

            default:
                StopMusic();
                break;
        }
    }

    // MUSIC
    public void PlayMusic(int index, bool loop = true)
    {
        if (index < 0 || index >= musicTracks.Length) return;

        // Prevent restarting same track
        if (musicSource.clip == musicTracks[index] && musicSource.isPlaying)
            return;

        musicSource.clip = musicTracks[index];
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // SOUND EFFECTS
    public void PlaySFX(int index)
    {
        if (index < 0 || index >= sfxClips.Length) return;

        sfxSource.PlayOneShot(sfxClips[index]);
    }
}