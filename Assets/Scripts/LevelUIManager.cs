using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelUIManager : MonoBehaviour
{
    public static LevelUIManager Instance;

    public Text RingsAmount;
    public Text LivesAmount;
    public Text ScoreAmount;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
