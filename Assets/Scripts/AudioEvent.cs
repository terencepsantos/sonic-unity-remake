using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioEvent : MonoBehaviour
{
    private AudioSource audioSourceObj;


    void Awake()
    {
        audioSourceObj = GetComponent<AudioSource>();
    }


    void PlayOneShot(AudioClip audioClip)
    {
        audioSourceObj.PlayOneShot(audioClip);
        Invoke("DestroyAudioClip", audioClip.length + 0.1f);
    }


    void PlayOneShot(AudioClip audioClip, float volume)
    {
        audioSourceObj.PlayOneShot(audioClip, volume);
        Invoke("DestroyAudioClip", audioClip.length + 0.1f);
    }


    void PlayLoop(AudioClip audioClip)
    {
        audioSourceObj.clip = audioClip;
        audioSourceObj.loop = true;
        audioSourceObj.Play();
    }


    void DestroyAudioClip()
    {
        Destroy(gameObject);
    }
}
