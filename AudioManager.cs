using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{

    // private struct for adding name and audio clip in inspector
    [System.Serializable]
    private struct Sound
    {
        public string name;
        public AudioClip clip;
    }

    [Header("Bindings")]
    [SerializeField] private AudioSource source;
    [Header("Music and SFX Array")]
    [SerializeField] private Sound[] sounds;
    [Header("Loudest the Music Volume Can Be")]
    public float maxMusicVolume = 1f;
    private Dictionary<string, AudioClip> soundDictionary;

    public static AudioManager Instance;
    public float MaxMusicVolume
    {
        get { return Instance.maxMusicVolume; }
        set
        {
            if (maxMusicVolume != value)
            {
                maxMusicVolume = value;
                if (source != null)
                {
                    source.volume = value;
                }
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (source == null)
        {
            source = GetComponent<AudioSource>();
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        soundDictionary = new Dictionary<string, AudioClip>();

        foreach (Sound sound in sounds)
        {
            soundDictionary[sound.name] = sound.clip;
        }

        sounds = null;
    }

    public void PlayClip(AudioSource clipSource, string key)
    {
        if (soundDictionary.TryGetValue(key, out AudioClip clip))
        {
            clipSource.clip = clip;
            clipSource.Play();
        }
    }

    public void PlayClip(AudioSource clipSource, string key, float volume)
    {
        if (soundDictionary.TryGetValue(key, out AudioClip clip))
        {
            clipSource.clip = clip;
            clipSource.volume = volume;
            clipSource.Play();
        }
    }

    public void PlayMusic(string key)
    {
        if (soundDictionary.TryGetValue(key, out AudioClip clip))
        {
            source.clip = clip;
            source.Play();
        }
    }

    public void PlayMusic(string key, float volume)
    {
        if (soundDictionary.TryGetValue(key, out AudioClip clip))
        {
            source.clip = clip;
            source.volume = volume;
            source.Play();
        }
    }

    public void StopPlaying(string name)
    {
        source.Stop();
    }

    public void ScaleVolume(float scaler)
    {
        source.volume = maxMusicVolume * scaler;
    }
    
    public void ResetVolume()
    {
        source.volume = maxMusicVolume;
    }
}
