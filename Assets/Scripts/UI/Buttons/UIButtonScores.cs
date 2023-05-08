using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonScores : UIButton
{
    protected override void OnButtonClick()
    {
        SceneManager.LoadScene("HighScores");
    }
}
