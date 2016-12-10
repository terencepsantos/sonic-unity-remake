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
            var go = Instantiate(FadeInPrefab) as GameObject;
            Destroy(go, FadeDurationInSeconds + 0.1f);
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


    public void LoadOtherScene(string levelToLoad, bool loadNextLevelThruMyLoading)
    {
        if (loadNextLevelThruMyLoading)
            MyLoading.LoadLevel(levelToLoad);
        else
            SceneManager.LoadScene(levelToLoad);
    }


    public void WaitAndLoadOtherScene(string levelToLoad, float secondsToWait, bool loadNextLevelThruMyLoading)
    {
        StartCoroutine(WaitAndLoadOtherSceneIE(levelToLoad, secondsToWait, loadNextLevelThruMyLoading));
    }

    private IEnumerator WaitAndLoadOtherSceneIE(string levelToLoad, float secondsToWait, bool loadNextLevelThruMyLoading)
    {
        yield return new WaitForSeconds(secondsToWait);

        if (loadNextLevelThruMyLoading)
            MyLoading.LoadLevel(levelToLoad);
        else
            SceneManager.LoadScene(levelToLoad);
    }


    public void FadeOut()
    {
        var go = Instantiate(FadeOutPrefab) as GameObject;
        Destroy(go, FadeDurationInSeconds + 0.1f);
        Invoke("LoadOtherScene", FadeDurationInSeconds);
    }
}
