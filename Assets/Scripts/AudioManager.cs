using UnityEngine;
using System.Collections;

public class AudioManager : Singleton<AudioManager>
{
    //public GameObject AudioEventPrefab;

    public AudioClip[] AudioClipsArray;
    public enum AudioClipsEnum
    {
        Sega,
        Level1BG,
        Jump,
        BeginCharge,
        ReleaseCharge,
        Break,
        RingCollect,
        LifeUp,
        TakeDamage,
        KillEnemy
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


    public void WaitAndPlayOneShot(AudioClipsEnum audioClip, float secondsToWait)
    {
        StartCoroutine(PlayOneShot(audioClip, secondsToWait));
    }


    private IEnumerator PlayOneShot(AudioClipsEnum audioClip, float secondsToWait)
    {
        var go = new GameObject("AudioEvent");
        go.AddComponent<AudioEvent>();

        yield return new WaitForSeconds(secondsToWait);

        go.SendMessage("PlayOneShot", AudioClipsArray[(int)audioClip]);
    }

    //TODO: Create AudioSetup class in order to send clip and optionally a set of values (volume etc.)

    //public void PlayOneShot(AudioClipsEnum audioClip, float volume)
    //{
    //    var go = new GameObject("AudioEvent");
    //    go.AddComponent<AudioEvent>();
    //    go.SendMessage("PlayOneShot", AudioClipsArray[(int)audioClip]);

    //}
}
