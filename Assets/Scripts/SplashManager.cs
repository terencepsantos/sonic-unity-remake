using UnityEngine;
using System.Collections;

public class SplashManager : MonoBehaviour
{
    [SerializeField]
    private float secondsToPlaySound = 0.5f;

	void Start ()
    {
        AudioManager.Instance.WaitAndPlayOneShot(AudioManager.AudioClipsEnum.Sega, 0.5f);
	}

}
