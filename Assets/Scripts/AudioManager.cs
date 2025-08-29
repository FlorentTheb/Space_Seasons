using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private List<AudioSource> Musics;
    [SerializeField] private List<AudioSource> Sounds;
    [SerializeField] private float fadeDuration = 1f;
    private int currentMusicIndex;
    private float CurrentMusicVolume;
    private float CurrentSoundVolume;
    private Coroutine fadeMusicRoutine;

    void Awake()
    {
        currentMusicIndex = 0;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            float vol = PlayerPrefs.GetFloat("Volume", 100f);
            SetMusicVolume(vol);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        foreach (var music in Musics)
        {
            music.loop = true;
        }
        Musics[0].Play();
        Musics[1].Stop();
    }

    public void SetMusicVolume(float newVolume)
    {
        foreach (var music in Musics)
        {
            music.volume = newVolume;
        }
        CurrentMusicVolume = newVolume;
    }

    public void CrossfadeMusic(int newMusicIndex)
    {
        if (fadeMusicRoutine != null)
            StopCoroutine(fadeMusicRoutine);

        if (newMusicIndex != currentMusicIndex && newMusicIndex >= 0 && newMusicIndex < Musics.Count)
            fadeMusicRoutine = StartCoroutine(FadeMusic(newMusicIndex));
    }

    private IEnumerator FadeMusic(int newMusicIndex)
    {
        Musics[newMusicIndex].Play();

        float time = 0f;
        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            Musics[currentMusicIndex].volume = Mathf.Lerp(CurrentMusicVolume, 0f, t);
            Musics[newMusicIndex].volume = Mathf.Lerp(0f, CurrentMusicVolume, t);
            time += Time.deltaTime;
            yield return null;
        }

        Musics[currentMusicIndex].volume = 0f;
        Musics[currentMusicIndex].Stop();
        Musics[newMusicIndex].volume = CurrentMusicVolume;

        currentMusicIndex = newMusicIndex;
    }
}
