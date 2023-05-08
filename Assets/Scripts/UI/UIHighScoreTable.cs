using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHighScoreTable : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoresText;
    [SerializeField] private TextMeshProUGUI playTimeText;

    void Start()
    {
        highScoresText.text = GameManager.GetHighScores();
        playTimeText.text = GameManager.GetPlayTime();
    }
}
