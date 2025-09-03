using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private List<AudioSource> Musics;
    [SerializeField] private List<AudioSource> Sounds;
    [SerializeField] private float fadeFactor = .2f;
    [SerializeField] private float TargetMusicVolume;
    [SerializeField] private float TargetSFXVolume;
    [SerializeField] private int currentMusicIndex = 0;
    [SerializeField] private int targetMusicIndex = -1;
    private Coroutine fadeMusicRoutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            float targetVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            SetMusicVolume(targetVolume);
            targetVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            SetSFXVolume(targetVolume);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < Musics.Count; i++)
        {
            var music = Musics[i];  
            music.loop = true;
            if (i == currentMusicIndex)
            {
                music.Play();
                music.volume = TargetMusicVolume;
                targetMusicIndex = currentMusicIndex;
            }
            else
            {
                music.Stop();
                music.volume = 0f;
            }
        }


        for (int i = 0; i < Sounds.Count; i++)
        {
            var sound = Sounds[i];
            sound.loop = false;
        }
    }

    public void PlayCollect()
    {
        PlaySound(0);
    }

    public void PlayHit()
    {
        PlaySound(1);
    }

    private void PlaySound(int soundIndex)
    {
        Debug.Log($"Sound played | Index = {soundIndex}");
        if (soundIndex >= 0 && soundIndex < Sounds.Count)
            Sounds[soundIndex].Play();
    }

    public void SetMusicVolume(float newVolume)
    {
        TargetMusicVolume = Mathf.Clamp01(newVolume);

        for (int i = 0; i < Musics.Count; i++)
            Musics[i].volume = (i == targetMusicIndex) ? TargetMusicVolume : 0f;
    }

    public void SetSFXVolume(float newVolume)
    {
        TargetSFXVolume = Mathf.Clamp01(newVolume);

        for (int i = 0; i < Sounds.Count; i++)
            Sounds[i].volume = TargetSFXVolume;
    }

    public void CrossfadeMusic(int newMusicIndex)
    {
        if (newMusicIndex < 0 || newMusicIndex >= Musics.Count || newMusicIndex == targetMusicIndex)
            return;

        currentMusicIndex = targetMusicIndex;
        targetMusicIndex = newMusicIndex;

        if (fadeMusicRoutine == null)
            fadeMusicRoutine = StartCoroutine(FadeMusicRoutine());
    }

    private IEnumerator FadeMusicRoutine()
    {
        Musics[targetMusicIndex].Play();
        while (Musics[targetMusicIndex].volume < TargetMusicVolume || Musics[currentMusicIndex].volume > 0f)
        {
            float delta = Time.timeScale > 0f ? Time.deltaTime : Time.unscaledDeltaTime;
            if (Musics[targetMusicIndex].volume < TargetMusicVolume)
            {
                Musics[targetMusicIndex].volume += delta * fadeFactor;
            }

            if (Musics[targetMusicIndex].volume > TargetMusicVolume)
                Musics[targetMusicIndex].volume = TargetMusicVolume;

            if (Musics[currentMusicIndex].volume > 0f)
            {
                Musics[currentMusicIndex].volume -= delta * fadeFactor;
            }

            if (Musics[currentMusicIndex].volume < 0f)
                Musics[currentMusicIndex].volume = 0;

            yield return null;
        }
        Musics[currentMusicIndex].Stop();
        fadeMusicRoutine = null;
    }
}
