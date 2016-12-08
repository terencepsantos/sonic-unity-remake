using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour 
{
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
