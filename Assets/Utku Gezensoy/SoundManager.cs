using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private bool _isSoundOn = true;

    public AudioSource btnSfxSource;
    public AudioClip btnSfxClip;

    public AudioClip MenuBGM;
    public AudioClip Level1BGM;
    public AudioClip Level2BGM;
    public AudioClip Level3BGM;
    public AudioClip Level4BGM;
    public AudioClip Level5BGM;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        PlayLevelBGM();
    }

    private void Start()
    {
        if (btnSfxSource == null)
        {
            btnSfxSource = gameObject.AddComponent<AudioSource>();
            //btnSfxSource.clip = btnSfxClip;
            //btnSfxSource.Play();

        }
    }

    public void PlayLevelBGM()
    {
        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
            btnSfxSource.clip = MenuBGM;
        }
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            btnSfxSource.clip = Level1BGM;
        }
        if (SceneManager.GetActiveScene().name == "Level2")
        {
            btnSfxSource.clip = Level2BGM;
        }
        if (SceneManager.GetActiveScene().name == "Level3")
        {
            btnSfxSource.clip = Level3BGM;
        }
        if (SceneManager.GetActiveScene().name == "Level4")
        {
            btnSfxSource.clip = Level4BGM;
        }
        if (SceneManager.GetActiveScene().name == "Level5")
        {
            btnSfxSource.clip = Level5BGM;
        }

        btnSfxSource.Play();
    }

    public void ToggleSound()
    {
        _isSoundOn = !_isSoundOn;

        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.volume = _isSoundOn ? 0.1f : 0f;
        }
    }

    public bool IsSoundOn()
    {
        return _isSoundOn;
    }

    public void PlayBtnSfx()
    {
        btnSfxSource.PlayOneShot(btnSfxClip);
    }
}