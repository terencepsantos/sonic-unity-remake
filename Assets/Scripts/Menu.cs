using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
    void Start()
    {
        AudioManager.Instance.PlayLoop(AudioManager.AudioClipsEnum.MenuBG);
    }

    public void ButtonPlay()
    {
        MyLoading.LoadLevel("03_Level");
    }

    public void ButtonOptions()
    {
        MyLoading.LoadLevel("Options");
    }

    public void ButtonBackToMenu()
    {
        MyLoading.LoadLevel("Menu");
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }
}
