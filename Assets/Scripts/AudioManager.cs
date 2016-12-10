using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{
    private List<GameObject> audioLoopsList;

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
        KillEnemy,
        MenuBG,
        PlayerDeath,
        EndLevelSign,
        EndLevelTheme
    }


    public override void Awake()
    {
        base.Awake();
        audioLoopsList = new List<GameObject>();
    }


    void OnLevelWasLoaded()
    {
        audioLoopsList.Clear();
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
        audioLoopsList.Add(go);
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


    public void StopLoops()
    {
        for (int i = 0; i < audioLoopsList.Count; i++)
        {
            audioLoopsList[i].SendMessage("DestroyAudioEvent");
        }

        audioLoopsList.Clear();
    }

    //TODO: Create AudioSetup class in order to send clip and optionally a set of values (volume etc.)

    //public void PlayOneShot(AudioClipsEnum audioClip, float volume)
    //{
    //    var go = new GameObject("AudioEvent");
    //    go.AddComponent<AudioEvent>();
    //    go.SendMessage("PlayOneShot", AudioClipsArray[(int)audioClip]);

    //}
}
