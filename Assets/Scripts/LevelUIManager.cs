using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelUIManager : MonoBehaviour
{
    public static LevelUIManager Instance;

    public Text RingsAmount;
    public Text LivesAmount;
    public Text ScoreAmount;

    public Text ThanksForPlaying;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        ThanksForPlaying.gameObject.SetActive(false);
    }
}
