using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class Options : MonoBehaviour 
{
    public Slider MusicVolumeSlider;
    public AudioMixer GameMixer;

    public void VolumeChange()
    {
        GameMixer.SetFloat("Volume", MusicVolumeSlider.value);
    }


    public void VolumeChange2(Slider slider)
    {
        GameMixer.SetFloat("Volume", slider.value);
    }
}
