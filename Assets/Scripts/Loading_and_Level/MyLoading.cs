using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MyLoading
{
    public static string LevelToLoad;

    public static void LoadLevel(string level)
    {
        LevelToLoad = level;

        SceneManager.LoadScene("Loading");
    }

}
