using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonMenu : UIButton
{
    protected override void OnButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
