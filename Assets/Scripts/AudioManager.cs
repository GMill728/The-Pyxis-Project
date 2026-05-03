using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Audio;

public enum MusicType
{
    MainMenu,
    InGame,

}

public enum SFXType
{
    PlayerBoosters,
    LaserShot,
    Pickup,
    BulletImpact,
    EnemyKill,
    ButtonClick,
    ButtonClick2,

}

[System.Serializable]
public class MusicEntry
{
    public MusicType type;
    public AudioClip clip;
}

[System.Serializable]
public class SFXEntry
{
    public SFXType type;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioMixerGroup sfxMixerGroup;

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfx2DSource;

    [Header("Audio Library")]
    public MusicEntry[] musicEntries;
    public SFXEntry[] sfxEntries;


    [Header("3D Audio Pool")]
    public Transform audioPoolParent;
    public int poolSize = 15;

    private Dictionary<MusicType, AudioClip> musicDict;
    private Dictionary<SFXType, AudioClip> sfxDict;

    private Queue<AudioSource> pool = new Queue<AudioSource>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            BuildDictionaries();
            BuildPool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void BuildDictionaries()
    {
        musicDict = new Dictionary<MusicType, AudioClip>();
        foreach (var entry in musicEntries)
            if (!musicDict.ContainsKey(entry.type))
                musicDict.Add(entry.type, entry.clip);

        sfxDict = new Dictionary<SFXType, AudioClip>();
        foreach (var entry in sfxEntries)
            if (!sfxDict.ContainsKey(entry.type))
                sfxDict.Add(entry.type, entry.clip);
    }

void BuildPool()
{
    for (int i = 0; i < poolSize; i++)
    {
        GameObject obj = new GameObject("PooledAudio_" + i);
        obj.transform.SetParent(audioPoolParent);

        AudioSource source = obj.AddComponent<AudioSource>();

        source.playOnAwake = false;
        source.spatialBlend = 1f;

        source.outputAudioMixerGroup = sfxMixerGroup;

        pool.Enqueue(source);
    }
}

    AudioSource GetPooledSource()
    {
        if (pool.Count > 0)
            return pool.Dequeue();

        // out of pools
        GameObject obj = new GameObject("ExtraAudio");
        AudioSource source = obj.AddComponent<AudioSource>();
        source.spatialBlend = 1f;
        return source;
    }

    void ReturnToPool(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        source.transform.position = Vector3.zero;
        source.transform.parent = transform;

        pool.Enqueue(source);
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
                PlayMusic(MusicType.MainMenu);
                break;

            case "ProcGen Test":
                PlayMusic(MusicType.InGame);
                break;

            default:
                StopMusic();
                break;
        }
    }



    public void PlayMusic(MusicType type, bool loop = true)
    {
        if (!musicDict.ContainsKey(type)) return;

        AudioClip clip = musicDict[type];

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }



    public void PlaySFX(SFXType type)
    {
        if (!sfxDict.ContainsKey(type)) return;

        sfx2DSource.PlayOneShot(sfxDict[type]);
    }



    public void PlaySFXAtPosition(SFXType type, Vector3 position, float minDist = 1f, float maxDist = 20f)
    {
        if (!sfxDict.ContainsKey(type)) return;

        AudioSource source = GetPooledSource();

        source.transform.position = position;
        source.clip = sfxDict[type];
        source.minDistance = minDist;
        source.maxDistance = maxDist;
        source.spatialBlend = 1f;

        source.pitch = Random.Range(0.7f, 1.3f);

        source.Play();
        StartCoroutine(ReturnAfterPlay(source));
    }

    System.Collections.IEnumerator ReturnAfterPlay(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        ReturnToPool(source);
    }
}