using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] AudioSource effectPlayer = null;
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
        bgmPlayer.Play();
    }
    public void PlayEffect(string path)
    {
        if (effectPlayer == null)
            effectPlayer = gameObject.AddComponent<AudioSource>();

        var clip = Resource.LoadAudioClip(path);
        effectPlayer.PlayOneShot(clip);
    }
}
