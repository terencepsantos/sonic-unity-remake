using UnityEngine;
using System.Collections;

public static class GameManager
{
    public static int LevelLoadCount = 0;
    public static int LivesAmount { get; private set; }
    public static int ScoreAmount { get; private set; }
    public static int RingsAmount { get; private set; }


    public static void SetPlayerCurrentStatus(int lives, int score, int rings)
    {
        LivesAmount = lives;
        ScoreAmount = score;
        RingsAmount = rings;
    }

}
