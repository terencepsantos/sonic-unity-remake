using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    public int LivesAmount { get; private set; }
    public int ScoreAmount { get; private set; }
    public int RingsAmount { get; private set; }


    void Awake()
    {
        SetPlayerCurrentStatus(0, 0, 0);
    }


    public void SetPlayerCurrentStatus(int lives, int score, int rings)
    {
        LivesAmount = lives;
        ScoreAmount = score;
        RingsAmount = rings;
    }

}
