using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChangeLevel : MonoBehaviour
{
    public float SecondsToOtherLevel = 2;
    public string LevelToLoad;

    [Space(10)]
    public bool LoadNextLevelThruMyLoading = true;
    public bool LoadNextLevelAutomatically = true;

    [Header("Fade In/Out Stuff")]
    public float FadeDurationInSeconds = 1.0f;
    public bool LoadNextLevelWithFade = false;
    public bool LoadThisLevelWithFade = false;
    public GameObject FadeOutPrefab;
    public GameObject FadeInPrefab;


    void Awake()
    {
        if (LoadThisLevelWithFade)
        {
            SecondsToOtherLevel += FadeDurationInSeconds;
            Instantiate(FadeInPrefab);
        }
    }


    void Start()
    {
        if (LoadNextLevelAutomatically)
        {
            if (!LoadNextLevelWithFade)
                Invoke("LoadOtherScene", SecondsToOtherLevel);
            else
                Invoke("FadeOut", SecondsToOtherLevel);
        }
    }


    private void LoadOtherScene()
    {
        if (LoadNextLevelThruMyLoading)
            MyLoading.LoadLevel(LevelToLoad);
        else
            SceneManager.LoadScene(LevelToLoad);
    }


    public void FadeOut()
    {
        Instantiate(FadeOutPrefab);
        Invoke("LoadOtherScene", FadeDurationInSeconds);
    }
}
