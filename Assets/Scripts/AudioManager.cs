using UnityEngine;
using System.Collections;

public class AudioManager : Singleton<AudioManager>
{
    //public GameObject AudioEventPrefab;

    public AudioClip[] AudioClipsArray;
    public enum AudioClipsEnum
    {
        Level1BG,
        Jump,
        BeginCharge,
        ReleaseCharge,
        Break,
        RingCollect,
        LifeUp
    }

    public void PlayOneShot(AudioClipsEnum audioClip)
    {
        var go = new GameObject("AudioEvent");
        go.AddComponent<AudioEvent>();
        go.SendMessage("PlayOneShot", AudioClipsArray[(int)audioClip]);

    }

    public void PlayLoop(AudioClipsEnum audioClip)
    {
        var go = new GameObject("AudioEvent");
        go.AddComponent<AudioEvent>();
        go.SendMessage("PlayLoop", AudioClipsArray[(int)audioClip]);
    }


    //TODO: Create AudioSetup class in order to send clip and optionally a set of values (volume etc.)

    //public void PlayOneShot(AudioClipsEnum audioClip, float volume)
    //{
    //    var go = new GameObject("AudioEvent");
    //    go.AddComponent<AudioEvent>();
    //    go.SendMessage("PlayOneShot", AudioClipsArray[(int)audioClip]);

    //}
}
