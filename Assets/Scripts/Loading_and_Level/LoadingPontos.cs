using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingPontos : MonoBehaviour
{
    public Text LoadingPontosText;
    public int FrameAmountToPoint = 30;
    private int count = 0;

    void Start()
    {
        LoadingPontosText.text = "";
    }

    void Update()
    {
        count++;

        if (count > FrameAmountToPoint * 1)
        {
            LoadingPontosText.text = ".";
        }

        if (count > FrameAmountToPoint * 2)
        {
            LoadingPontosText.text = "..";
        }

        if (count > FrameAmountToPoint * 3)
        {
            LoadingPontosText.text = "...";
        }

        if (count > FrameAmountToPoint * 4)
        {
            SceneManager.LoadSceneAsync(MyLoading.LevelToLoad);

            LoadingPontosText.text = "";
            count = 0;
        }

    }
}
