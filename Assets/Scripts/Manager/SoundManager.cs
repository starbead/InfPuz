using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] AudioSource effectPlayer = null;


    float defaultBgmValue = 0.3f;
    protected override void ChildAwake()
    {
        DontDestroyOnLoad(gameObject);
    }
    protected override void ChildOnDestroy()
    {
    }
    public void PlayBGM(string path)
    {
        if (bgmPlayer == null)
            bgmPlayer = gameObject.AddComponent<AudioSource>();

        bgmPlayer.clip = Resource.LoadAudioClip(path);
        bgmPlayer.loop = true;
        bgmPlayer.volume = defaultBgmValue * GameOption.BgmSound;
        bgmPlayer.Play();
    }
    public void PlayEffect(string path)
    {
        if (effectPlayer == null)
            effectPlayer = gameObject.AddComponent<AudioSource>();

        var clip = Resource.LoadAudioClip(path);
        effectPlayer.volume = GameOption.EffectSound;
        effectPlayer.PlayOneShot(clip);
    }
    public void SetBGMVolume(float value)
    {
        if (bgmPlayer == null)
            bgmPlayer = gameObject.AddComponent<AudioSource>();

        bgmPlayer.volume = defaultBgmValue * value;
    }
    public void SetEffectVolume(float value)
    {
        if (effectPlayer == null)
            effectPlayer = gameObject.AddComponent<AudioSource>();

        effectPlayer.volume = value;
    }
}
