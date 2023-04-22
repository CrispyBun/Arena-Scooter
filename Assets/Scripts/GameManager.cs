using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIScoreDisplay scoreDisplay;

    private static int score;
    private static bool scoreAdded = false;

    private void Update()
    {
        if (scoreAdded)
        {
            scoreDisplay.UpdateScore(score);
            scoreAdded = false;
        }
    }

    public static void AddScore(int addedScore)
    {
        score += addedScore;
        scoreAdded = true;
    }
}
