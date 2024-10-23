using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [SerializeField] private Sound[] effectSounds;
    [SerializeField] private Sound[] bgSounds;

    [SerializeField] private bool updateBgMusicOnSceneChange = false;

    private int currBgMusicIndex = -1;      // set -1 to signify no song playing

    private float mVol;     // Global music volume
    private float eVol;     // Global effects volume

    [SerializeField] private bool AutoStartBgMusic = true;
    private bool isMuted = false;
    private bool isBgMusicPlaying = false;
    
    private bool isGameFocused = true;
    private bool isCurrBgMusicOnLoop = false;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.Log($"{Instance.name} {gameObject.name} An instance of this singleton already exists");
        }
        else
        {
            Instance = this;
            
            DontDestroyOnLoad(this);
        }
    }
    
    void Start() 
    {
        // get preferences
        mVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        eVol = PlayerPrefs.GetFloat("EffectsVolume", 1f);
        
        createAudioSources(effectSounds, eVol);     // create sources for effects
        createAudioSources(bgSounds, mVol);   // create sources for music
        
        if (updateBgMusicOnSceneChange)
        {
            SceneManager.activeSceneChanged += ChangeBgMusicOnSceneChange;
            Application.focusChanged += OnApplicationFocusChanged;
        }
        
        if (AutoStartBgMusic)
            PlayMusic(true);
        
        SetMuteState();
    }

    void OnDestroy()
    {
        if (updateBgMusicOnSceneChange)
        {
            SceneManager.activeSceneChanged -= ChangeBgMusicOnSceneChange;
            Application.focusChanged -= OnApplicationFocusChanged;
        }
    }

    void Update() 
    {
        if (isBgMusicPlaying && isGameFocused && !isCurrBgMusicOnLoop) 
        {
            // // if we are playing a track (which wasn't on loop) from the playlist & it has stopped playing
            if (!bgSounds[currBgMusicIndex].source.isPlaying)
            {
                PlayMusic(true);
            }
        }
    }
    
    private void OnApplicationFocusChanged(bool hasFocus)
    {
        isGameFocused = hasFocus;
    }
    
    private void ChangeBgMusicOnSceneChange(Scene curr, Scene next)
    {
        if (!isGameFocused)
            return;
        
        PlayMusic(true);
    }
    
    private void createAudioSources(Sound[] sounds, float volume) 
    {
        foreach (Sound s in sounds) 
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void PlaySound(AudioType audioName) 
    {
        Sound s = Array.Find(effectSounds, sound => sound.name == audioName);
        
        if (s == null) 
        {
            Debug.LogWarning("Unable to play sound " + audioName);
            return;
        }
        
        s.source.Play();
    }
    
    public void StopSound(AudioType audioName)
    {
        Sound s = Array.Find(effectSounds, sound => sound.name == audioName);
        
        if (s == null)
        {
            Debug.LogWarning("Sound: " + audioName + " not found");
            return;
        }
        
        s.source.Stop();
    }
    
    public void PlayMusic(bool random, AudioType audioName = AudioType.None, float volReduceFactor = 1f)
    {
        if (bgSounds.Length == 0)
            return;
        
        // If already playing on Music, stop it first
        if (currBgMusicIndex != -1)
        {
            StopMusic();
        }
        
        if (random || audioName == AudioType.None)
        {
            if (bgSounds.Length > 1)
            {
                // pick a random song from our playlist & make sure it's not the same as prev one
                int index;
                do
                {
                    index = GetRandomBgMusicIndex();
                } 
                while (index == currBgMusicIndex);
                
                currBgMusicIndex = index;
            }
        }
        else
        {
            currBgMusicIndex = Array.FindIndex(bgSounds, sound => sound.name == audioName);

            if (currBgMusicIndex < 0)
            {
                // pick a random song from our playlist
                currBgMusicIndex = GetRandomBgMusicIndex();
            }
        }

        bgSounds[currBgMusicIndex].source.time = 0;
        bgSounds[currBgMusicIndex].source.volume = bgSounds[currBgMusicIndex].volume / volReduceFactor;
        bgSounds[currBgMusicIndex].source.Play();
        
        isBgMusicPlaying = true;
        isCurrBgMusicOnLoop = bgSounds[currBgMusicIndex].source.loop;
    }

    public void ReduceBgMusicSourceVolume(float volReduceFactor)
    {
        bgSounds[currBgMusicIndex].source.volume = bgSounds[currBgMusicIndex].volume / volReduceFactor;
    }

    public void StopMusic()
    {
        bgSounds[currBgMusicIndex].source.Stop();
        isBgMusicPlaying = false;
    }

    private int GetRandomBgMusicIndex()
    {
        return UnityEngine.Random.Range(0, bgSounds.Length);
    }
    
    public AudioType GetCurrBgSongName() 
    {
        return bgSounds[currBgMusicIndex].name;
    }

    // if the music volume change update all the audio sources
    public void OnMusicVolumeChanged(float val)
    {
        mVol = val;
        PlayerPrefs.SetFloat("MusicVolume", mVol);
        
        foreach (Sound m in bgSounds) 
        {
            m.source.volume = m.volume * mVol;
        }
    }

    // if the effects volume changed update the audio sources
    public void OnEffectVolumeChanged(float val) 
    {
        eVol = val;
        PlayerPrefs.SetFloat("EffectsVolume", eVol);
        
        foreach (Sound s in effectSounds) 
        {
            s.source.volume = s.volume * eVol;
        }
    }
    
    private void SetMuteState()
    {
        isMuted = PlayerPrefs.GetInt("Muted") == 1;
        SetAudioListener();
    }
    
    public void ToggleMuteState()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        SetAudioListener();
    }

    private void SetAudioListener()
    {
        AudioListener.pause = isMuted;
    }
}