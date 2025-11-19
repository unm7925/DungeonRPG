
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] audioClips;
    
    private Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // 단일 씬으로 인한 주석 처리
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        foreach (AudioClip clip in audioClips)
        {
            audioDict.Add(clip.name, clip);
        }
    }
    

    public void PlaySFX(string clipName,float volume)
    {
        
        if (audioDict.ContainsKey(clipName))
        {
            sfxSource.PlayOneShot(audioDict[clipName],volume);
        }
        else
        {
            Debug.Log("Audio clip not found: " + clipName);
        }
    }

    public void PlayBGM(string clipName)
    {
        if (audioDict.ContainsKey(clipName))
        {
            bgmSource.clip = audioDict[clipName];
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        { 
            Debug.Log("Audio clip not found: " + clipName);
        }
    }

    public void StopBgm()
    {
        bgmSource.Stop();
    }
    
}